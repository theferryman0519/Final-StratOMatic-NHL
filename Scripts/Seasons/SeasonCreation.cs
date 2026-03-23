// Main Dependencies
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

// Game Dependencies
using SoM.Controllers;
using SoM.Models;

namespace SoM.Seasons {
public class SeasonCreation : MonoBehaviour {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    private string seasonId = string.Empty;
    private string userTeam = string.Empty;
    private string userLeague = string.Empty;

    private int gameNight = 0;
    private int version = 0;

    private SemaphoreSlim createSeasonLock = new(1, 1);
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public async Task<Season> CreateSeason(SeasonDatabase seasonDatabase)
    {
        await createSkaterLock.WaitAsync();
        try
        {
            CoreController.Inst.WriteLog(this.GetType().Name, $"Creating the season.");

            seasonId = seasonDatabase.Id;

            Season newSeason = new Season
            {
                Id = seasonDatabase.Id,
                League = seasonDatabase.League,
                Version = seasonDatabase.Version,
            };

            newSeason.Team = await GetUserTeam(seasonDatabase.Team, seasonDatabase.League);
            newSeason.GameNights = new();

            userTeam = newSeason.Team.Info.Code;
            userLeague = newSeason.League;

            version = seasonDatabase.Version;
            gameNight = seasonDatabase.GameNight;

            newSeason.GameNights = await SetGameNights();

            CoreController.Inst.WriteLog(this.GetType().Name, $"Season data has been created.");
            return newSeason;
        }
        finally
        {
            createSkaterLock.Release();
        }
    }
#endregion
#region -------------------- Private Methods --------------------
    private async Task<Team> GetUserTeam(string team, string league)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Getting the user team for the season.");

        if (string.IsNullOrEmpty(team) || string.IsNullOrEmpty(league))
        {
            return null;
        }

        // TODO
        return TeamsController.Inst.GetTeamByCode(team, league);
    }

    private async Task<List<GameNight>> SetGameNights()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting all game nights for the season.");

        List<GameNight> gameNights = new();

        for (int i = 0; i < 82; i++)
        {
            int index = i;

            GameNight newGameNight = await SetGameNight(index);
            
            gameNights.Add(newGameNight);
        }

        return gameNights;
    }

    private async Task<GameNight> SetGameNight(int index)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting each game night for the season.");

        GameNight gameNight = new GameNight
        {
            Number = index + 1,
            Games = new(),
        };

        for (int i = 0; i < 16; i++)
        {
            int gameIndex = i;

            Game newGame = await SetGame(index + 1, gameIndex);

            gameNight.Games.Add(newGame);
        }

        return gameNight;
    }

    private async Task<Game> SetGame(int nightIndex, int gameIndex)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Setting each game for the night.");

        string gameString = await GetGameString(nightIndex, gameIndex);
        string[] gameTeams = gameString.Split('_');

        Game game = new Game
        {
            Id = Guid.NewGuid().ToString(),
            Type = "Season",
            HomeUserType = (gameTeams[0] == userTeam) ? "User" : "Ai",
            AwayUserType = (gameTeams[1] == userTeam) ? "User" : "Ai",
            HomeTeam = await GetUserTeam(gameTeams[0], userLeague),
            AwayTeam = await GetUserTeam(gameTeams[1], userLeague),
        };

        return game;
    }

    private async Task<string> GetGameString(int nightIndex, int gameIndex)
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Getting a specific game string.");

        Dictionary<int, List<string>> gameStringsNhl = new();
        Dictionary<int, List<string>> gameStringsPwhl = new();

        switch (version)
        {
            case 2:
                gameStringsNhl[1].AddRange(new string[] { "DET_CAR", "FLA_BUF", "MTL_BOS", "OTT_WSH", "TBL_PIT", "TOR_PHI", "CHI_NYR", "COL_NYI", "NJD_DAL", "CBJ_MIN", "VGK_NSH", "VAN_STL", "SEA_UTA", "SJS_WPG", "LAK_ANA", "EDM_CGY" });
                gameStringsNhl[2].AddRange(new string[] { "CAR_TBL", "OTT_TOR", "MTL_CHI", "FLA_COL", "DET_DAL", "BUF_MIN", "BOS_NSH", "WSH_STL", "UTA_PIT", "WPG_PHI", "ANA_NYR", "CGY_NYI", "EDM_NJD", "LAK_CBJ", "SJS_VGK", "SEA_VAN" });
                gameStringsNhl[3].AddRange(new string[] { "CAR_CHI", "COL_TOR", "DAL_TBL", "MIN_OTT", "NSH_MTL", "STL_FLA", "UTA_DET", "WPG_BUF", "BOS_ANA", "WSH_CGY", "PIT_EDM", "PHI_LAK", "NYR_SJS", "NYI_SEA", "NJD_VAN", "CBJ_VGK" });
                gameStringsNhl[4].AddRange(new string[] { "FLA_CAR", "MTL_DET", "OTT_BUF", "TBL_BOS", "TOR_WSH", "CHI_PIT", "COL_PHI", "DAL_NYR", "MIN_NYI", "NJD_NSH", "CBJ_STL", "VGK_UTA", "VAN_WPG", "SEA_ANA", "SJS_CGY", "LAK_EDM" });
                gameStringsNhl[5].AddRange(new string[] { "CAR_PIT", "PHI_WSH", "NYR_BOS", "NYI_BUF", "NJD_DET", "CBJ_FLA", "VGK_MTL", "VAN_OTT", "TBL_SEA", "TOR_SJS", "CHI_LAK", "COL_EDM", "DAL_CGY", "MIN_ANA", "NSH_WPG", "STL_UTA" });
                gameStringsNhl[6].AddRange(new string[] { "BUF_CAR", "DET_BOS", "FLA_WSH", "MTL_PIT", "OTT_PHI", "TBL_NYR", "TOR_NYI", "CHI_NJD", "COL_CBJ", "VGK_DAL", "VAN_MIN", "SEA_NSH", "SJS_STL", "LAK_UTA", "EDM_WPG", "CGY_ANA" });
                gameStringsNhl[7].AddRange(new string[] { "CAR_MIN", "NSH_DAL", "STL_COL", "UTA_CHI", "WPG_TOR", "ANA_TBL", "CGY_OTT", "EDM_MTL", "LAK_FLA", "DET_SJS", "BUF_SEA", "BOS_VAN", "WSH_VGK", "PIT_CBJ", "PHI_NJD", "NYR_NYI" });
                gameStringsNhl[8].AddRange(new string[] { "CAR_VGK", "CBJ_VAN", "NJD_SEA", "NYI_SJS", "NYR_LAK", "PHI_EDM", "PIT_CGY", "WSH_ANA", "WPG_BOS", "UTA_BUF", "STL_DET", "NSH_FLA", "MIN_MTL", "DAL_OTT", "COL_TBL", "CHI_TOR" });
                gameStringsNhl[9].AddRange(new string[] { "CAR_UTA", "WPG_STL", "ANA_NSH", "CGY_MIN", "EDM_DAL", "LAK_COL", "SJS_CHI", "SEA_TOR", "TBL_VAN", "OTT_VGK", "MTL_CBJ", "FLA_NJD", "DET_NYI", "BUF_NYR", "BOS_PHI", "WSH_PIT" });
                gameStringsNhl[10].AddRange(new string[] { "CAR_PHI", "NYR_PIT", "NYI_WSH", "NJD_BOS", "CBJ_BUF", "VGK_DET", "VAN_FLA", "SEA_MTL", "OTT_SJS", "TBL_LAK", "TOR_EDM", "CHI_CGY", "COL_ANA", "DAL_WPG", "MIN_UTA", "NSH_STL" });
                gameStringsNhl[11].AddRange(new string[] { "CAR_NYI", "NJD_NYR", "CBJ_PHI", "PIT_WSH", "BOS_FLA", "DET_MTL", "BUF_OTT", "TOR_TBL", "CHI_MIN", "DAL_NSH", "COL_STL", "UTA_WPG", "ANA_LAK", "EDM_SJS", "CGY_SEA", "VGK_VAN" });
                gameStringsNhl[12].AddRange(new string[] { "NYR_CAR", "PHI_NYI", "PIT_NJD", "WSH_CBJ", "BOS_VGK", "BUF_VAN", "DET_SEA", "FLA_SJS", "LAK_MTL", "EDM_OTT", "CGY_TBL", "ANA_TOR", "WPG_CHI", "UTA_COL", "STL_DAL", "NSH_MIN" });
                gameStringsNhl[13].AddRange(new string[] { "DAL_CAR", "COL_MIN", "CHI_NSH", "TOR_STL", "TBL_UTA", "OTT_WPG", "MTL_ANA", "FLA_CGY", "EDM_DET", "LAK_BUF", "SJS_BOS", "SEA_WSH", "VAN_PIT", "VGK_PHI", "CBJ_NYR", "NJD_NYI" });
                gameStringsNhl[14].AddRange(new string[] { "UTA_CAR", "STL_WPG", "NSH_ANA", "MIN_CGY", "DAL_EDM", "COL_LAK", "CHI_SJS", "TOR_SEA", "VAN_TBL", "VGK_OTT", "CBJ_MTL", "NJD_FLA", "NYI_DET", "NYR_BUF", "PHI_BOS", "PIT_WSH" });
                gameStringsNhl[15].AddRange(new string[] { "CAR_NYR", "NYI_PHI", "NJD_PIT", "CBJ_WSH", "BOS_MTL", "FLA_OTT", "DET_TBL", "TOR_BUF", "CHI_NSH", "MIN_STL", "DAL_UTA", "COL_WPG", "ANA_SJS", "LAK_SEA", "EDM_VAN", "VGK_CGY" });
                gameStringsNhl[16].AddRange(new string[] { "CAR_PIT", "PHI_WSH", "NYR_BOS", "NYI_BUF", "DET_NJD", "FLA_CBJ", "MTL_TOR", "OTT_TBL", "CHI_UTA", "STL_WPG", "NSH_ANA", "MIN_CGY", "EDM_DAL", "LAK_COL", "SJS_VGK", "SEA_VAN" });
                gameStringsNhl[17].AddRange(new string[] { "SEA_CAR", "SJS_VAN", "LAK_VGK", "EDM_CBJ", "CGY_NJD", "ANA_NYI", "WPG_NYR", "UTA_PHI", "PIT_STL", "WSH_NSH", "BOS_MIN", "BUF_DAL", "DET_COL", "FLA_CHI", "MTL_TOR", "OTT_TBL" });
                gameStringsNhl[18].AddRange(new string[] { "CAR_DET", "BUF_FLA", "BOS_MTL", "WSH_OTT", "PIT_TBL", "PHI_TOR", "NYR_CHI", "NYI_COL", "DAL_NJD", "MIN_CBJ", "NSH_VGK", "STL_VAN", "UTA_SEA", "WPG_SJS", "ANA_LAK", "CGY_EDM" });
                gameStringsNhl[19].AddRange(new string[] { "CAR_WPG", "ANA_UTA", "CGY_STL", "EDM_NSH", "LAK_MIN", "SJS_DAL", "SEA_COL", "VAN_CHI", "VGK_TOR", "TBL_CBJ", "OTT_NJD", "MTL_NYI", "FLA_NYR", "DET_PHI", "BUF_PIT", "BOS_WSH" });
                gameStringsNhl[20].AddRange(new string[] { "CAR_ANA", "CGY_WPG", "EDM_UTA", "LAK_STL", "SJS_NSH", "SEA_MIN", "VAN_DAL", "VGK_COL", "CHI_CBJ", "TOR_NJD", "TBL_NYI", "OTT_NYR", "MTL_PHI", "FLA_PIT", "DET_WSH", "BUF_BOS" });
                gameStringsNhl[21].AddRange(new string[] { "CAR_NYR", "NYI_PHI", "NJD_PIT", "CBJ_WSH", "BOS_TOR", "BUF_TBL", "DET_OTT", "FLA_MTL", "CHI_NSH", "MIN_STL", "DAL_UTA", "COL_WPG", "ANA_VGK", "CGY_VAN", "EDM_SEA", "LAK_SJS" });
                gameStringsNhl[22].AddRange(new string[] { "WPG_CAR", "UTA_ANA", "STL_CGY", "NSH_EDM", "MIN_LAK", "DAL_SJS", "COL_SEA", "CHI_VAN", "TOR_VGK", "CBJ_TBL", "NJD_OTT", "NYI_MTL", "NYR_FLA", "PHI_DET", "PIT_BUF", "WSH_BOS" });
                gameStringsNhl[23].AddRange(new string[] { "CAR_NYI", "NJD_NYR", "CBJ_PHI", "TOR_PIT", "WSH_TBL", "BOS_OTT", "BUF_MTL", "DET_FLA", "CHI_MIN", "DAL_NSH", "COL_STL", "VGK_UTA", "WPG_VAN", "ANA_SEA", "CGY_SJS", "EDM_LAK" });
                gameStringsNhl[24].AddRange(new string[] { "CAR_TOR", "TBL_CHI", "OTT_COL", "MTL_DAL", "FLA_MIN", "DET_NSH", "BUF_STL", "BOS_UTA", "WSH_WPG", "ANA_PIT", "CGY_PHI", "EDM_NYR", "LAK_NYI", "SJS_NJD", "SEA_CBJ", "VAN_VGK" });
                gameStringsNhl[25].AddRange(new string[] { "NSH_CAR", "MIN_STL", "DAL_UTA", "COL_WPG", "CHI_ANA", "TOR_CGY", "TBL_EDM", "OTT_LAK", "SJS_MTL", "SEA_FLA", "VAN_DET", "VGK_BUF", "CBJ_BOS", "NJD_WSH", "NYI_PIT", "NYR_PHI" });
                gameStringsNhl[26].AddRange(new string[] { "WSH_CAR", "PIT_CBJ", "PHI_NJD", "NYR_NYI", "TOR_BOS", "TBL_BUF", "OTT_DET", "MTL_FLA", "WPG_CHI", "UTA_COL", "STL_DAL", "NSH_MIN", "VGK_ANA", "VAN_CGY", "SEA_EDM", "SJS_LAK" });
                gameStringsNhl[27].AddRange(new string[] { "ANA_CAR", "WPG_CGY", "UTA_EDM", "STL_LAK", "NSH_SJS", "MIN_SEA", "DAL_VAN", "COL_VGK", "CBJ_CHI", "NJD_TOR", "NYI_TBL", "NYR_OTT", "PHI_MTL", "PIT_FLA", "WSH_DET", "BOS_BUF" });
                gameStringsNhl[28].AddRange(new string[] { "VAN_CAR", "SEA_VGK", "SJS_CBJ", "LAK_NJD", "EDM_NYI", "CGY_NYR", "ANA_PHI", "WPG_PIT", "WSH_UTA", "BOS_STL", "BUF_NSH", "DET_MIN", "FLA_DAL", "MTL_COL", "OTT_CHI", "TBL_TOR" });
                gameStringsNhl[29].AddRange(new string[] { "CAR_STL", "UTA_NSH", "WPG_MIN", "ANA_DAL", "CGY_COL", "EDM_CHI", "LAK_TOR", "SJS_TBL", "SEA_OTT", "MTL_VAN", "FLA_VGK", "DET_CBJ", "BUF_NJD", "BOS_NYI", "WSH_NYR", "PIT_PHI" });
                gameStringsNhl[30].AddRange(new string[] { "CAR_DAL", "MIN_COL", "NSH_CHI", "STL_TOR", "UTA_TBL", "WPG_OTT", "ANA_MTL", "CGY_FLA", "DET_EDM", "BUF_LAK", "BOS_SJS", "WSH_SEA", "PIT_VAN", "PHI_VGK", "NYR_CBJ", "NYI_NJD" });
                gameStringsNhl[31].AddRange(new string[] { "CAR_WSH", "PIT_BOS", "PHI_BUF", "NYR_DET", "NYI_FLA", "NJD_MTL", "CBJ_OTT", "VGK_TBL", "TOR_VAN", "CHI_SEA", "COL_SJS", "DAL_LAK", "MIN_EDM", "NSH_CGY", "STL_ANA", "UTA_WPG" });
                gameStringsNhl[32].AddRange(new string[] { "CAR_BUF", "BOS_DET", "WSH_FLA", "PIT_MTL", "PHI_OTT", "NYR_TBL", "NYI_TOR", "NJD_CHI", "CBJ_COL", "DAL_VGK", "MIN_VAN", "NSH_SEA", "STL_SJS", "UTA_LAK", "WPG_EDM", "ANA_CGY" });
                gameStringsNhl[33].AddRange(new string[] { "CAR_NJD", "CBJ_NYI", "VGK_NYR", "VAN_PHI", "SEA_PIT", "SJS_WSH", "LAK_BOS", "EDM_BUF", "DET_CGY", "FLA_ANA", "MTL_WPG", "OTT_UTA", "TBL_STL", "TOR_NSH", "CHI_MIN", "COL_DAL" });
                gameStringsNhl[34].AddRange(new string[] { "CAR_MTL", "FLA_OTT", "DET_TBL", "BUF_TOR", "BOS_CHI", "WSH_COL", "PIT_DAL", "PHI_MIN", "NSH_NYR", "STL_NYI", "UTA_NJD", "WPG_CBJ", "ANA_VGK", "CGY_VAN", "EDM_SEA", "LAK_SJS" });
                gameStringsNhl[35].AddRange(new string[] { "OTT_CAR", "MTL_TBL", "FLA_TOR", "DET_CBJ", "NJD_BUF", "NYI_BOS", "NYR_WSH", "PHI_PIT", "SEA_CHI", "SJS_VAN", "LAK_VGK", "EDM_COL", "DAL_CGY", "MIN_ANA", "NSH_WPG", "STL_UTA" });
                gameStringsNhl[36].AddRange(new string[] { "CAR_CBJ", "VGK_NJD", "VAN_NYI", "SEA_NYR", "SJS_PHI", "LAK_PIT", "EDM_WSH", "CGY_BOS", "BUF_ANA", "DET_WPG", "FLA_UTA", "MTL_STL", "OTT_NSH", "TBL_MIN", "TOR_DAL", "CHI_COL" });
                gameStringsNhl[37].AddRange(new string[] { "TOR_CAR", "CHI_TBL", "COL_OTT", "DAL_MTL", "MIN_FLA", "NSH_DET", "STL_BUF", "UTA_BOS", "WPG_WSH", "PIT_ANA", "PHI_CGY", "NYR_EDM", "NYI_LAK", "NJD_SJS", "CBJ_SEA", "VGK_VAN" });
                gameStringsNhl[38].AddRange(new string[] { "CAR_SEA", "VAN_SJS", "VGK_LAK", "CBJ_EDM", "NJD_CGY", "NYI_ANA", "NYR_WPG", "PHI_UTA", "STL_PIT", "NSH_WSH", "MIN_BOS", "DAL_BUF", "COL_DET", "CHI_FLA", "TOR_MTL", "TBL_OTT" });
                gameStringsNhl[39].AddRange(new string[] { "CAR_NSH", "STL_MIN", "UTA_DAL", "WPG_COL", "ANA_CHI", "CGY_TOR", "EDM_TBL", "LAK_OTT", "MTL_SJS", "FLA_SEA", "DET_VAN", "BUF_VGK", "BOS_CBJ", "WSH_NJD", "PIT_NYI", "PHI_NYR" });
                gameStringsNhl[40].AddRange(new string[] { "CAR_BOS", "WSH_BUF", "PIT_DET", "PHI_FLA", "NYR_MTL", "NYI_OTT", "NJD_TBL", "CBJ_TOR", "CHI_VGK", "COL_VAN", "DAL_SEA", "MIN_SJS", "NSH_LAK", "STL_EDM", "UTA_CGY", "WPG_ANA" });
                gameStringsNhl[41].AddRange(new string[] { "MTL_CAR", "FLA_OTT", "DET_TBL", "BUF_TOR", "CBJ_BOS", "NJD_WSH", "NYI_PIT", "NYR_PHI", "SJS_CHI", "LAK_SEA", "EDM_VAN", "CGY_VGK", "COL_ANA", "DAL_WPG", "MIN_UTA", "NSH_STL" });
                gameStringsNhl[42].AddRange(new string[] { "CAR_NYR", "NYI_PHI", "NJD_PIT", "CBJ_WSH", "VGK_BOS", "VAN_BUF", "SEA_DET", "SJS_FLA", "MTL_LAK", "OTT_EDM", "TBL_CGY", "TOR_ANA", "CHI_WPG", "COL_UTA", "DAL_STL", "MIN_NSH" });
                gameStringsNhl[43].AddRange(new string[] { "SJS_CAR", "LAK_SEA", "EDM_VAN", "CGY_VGK", "ANA_CBJ", "WPG_NJD", "UTA_NYI", "STL_NYR", "PHI_NSH", "PIT_MIN", "WSH_DAL", "BOS_COL", "BUF_CHI", "DET_TOR", "FLA_TBL", "MTL_OTT" });
                gameStringsNhl[44].AddRange(new string[] { "EDM_CAR", "CGY_LAK", "ANA_SJS", "WPG_SEA", "UTA_VAN", "STL_VGK", "NSH_CBJ", "MIN_NJD", "NYI_DAL", "NYR_COL", "PHI_CHI", "PIT_TOR", "WSH_TBL", "BOS_OTT", "BUF_MTL", "DET_FLA" });
                gameStringsNhl[45].AddRange(new string[] { "CGY_CAR", "ANA_EDM", "WPG_LAK", "UTA_SJS", "STL_SEA", "NSH_VAN", "MIN_VGK", "DAL_CBJ", "NJD_COL", "NYI_CHI", "NYR_TOR", "PHI_TBL", "PIT_OTT", "WSH_MTL", "BOS_FLA", "BUF_DET" });
                gameStringsNhl[46].AddRange(new string[] { "CAR_OTT", "MTL_TBL", "FLA_TOR", "DET_CHI", "BUF_COL", "BOS_DAL", "WSH_MIN", "PIT_NSH", "PHI_STL", "UTA_NYR", "WPG_NYI", "ANA_NJD", "CGY_CBJ", "EDM_VGK", "LAK_VAN", "SJS_SEA" });
                gameStringsNhl[47].AddRange(new string[] { "PHI_CAR", "PIT_NYR", "WSH_NYI", "BOS_NJD", "BUF_CBJ", "DET_VGK", "FLA_VAN", "MTL_SEA", "SJS_OTT", "LAK_TBL", "EDM_TOR", "CGY_CHI", "ANA_COL", "WPG_DAL", "UTA_MIN", "STL_NSH" });
                gameStringsNhl[48].AddRange(new string[] { "CAR_FLA", "DET_MTL", "BUF_OTT", "BOS_TBL", "WSH_TOR", "PIT_CHI", "PHI_COL", "NYR_DAL", "NYI_MIN", "NSH_NJD", "STL_CBJ", "UTA_VGK", "WPG_VAN", "ANA_SEA", "CGY_SJS", "EDM_LAK" });
                gameStringsNhl[49].AddRange(new string[] { "TBL_CAR", "OTT_TOR", "MTL_CBJ", "FLA_NJD", "NYI_DET", "NYR_BUF", "PHI_BOS", "PIT_WSH", "VAN_CHI", "SEA_VGK", "SJS_COL", "LAK_DAL", "MIN_EDM", "NSH_CGY", "STL_ANA", "UTA_WPG" });
                gameStringsNhl[50].AddRange(new string[] { "OTT_CAR", "TBL_MTL", "TOR_FLA", "CHI_DET", "COL_BUF", "DAL_BOS", "MIN_WSH", "NSH_PIT", "STL_PHI", "NYR_UTA", "NYI_WPG", "NJD_ANA", "CBJ_CGY", "VGK_EDM", "VAN_LAK", "SEA_SJS" });
                gameStringsNhl[51].AddRange(new string[] { "CAR_PHI", "NYR_PIT", "NYI_WSH", "NJD_BOS", "CBJ_BUF", "DET_TOR", "FLA_TBL", "MTL_OTT", "CHI_STL", "NSH_UTA", "MIN_WPG", "DAL_ANA", "COL_CGY", "EDM_VGK", "LAK_VAN", "SJS_SEA" });
                gameStringsNhl[52].AddRange(new string[] { "CAR_NYI", "NJD_NYR", "CBJ_PHI", "VGK_PIT", "VAN_WSH", "SEA_BOS", "SJS_BUF", "LAK_DET", "FLA_EDM", "MTL_CGY", "OTT_ANA", "TBL_WPG", "TOR_UTA", "CHI_STL", "COL_NSH", "DAL_MIN" });
                gameStringsNhl[53].AddRange(new string[] { "LAK_CAR", "EDM_SJS", "CGY_SEA", "ANA_VAN", "WPG_VGK", "UTA_CBJ", "STL_NJD", "NSH_NYI", "NYR_MIN", "PHI_DAL", "PIT_COL", "WSH_CHI", "BOS_TOR", "BUF_TBL", "DET_OTT", "FLA_MTL" });
                gameStringsNhl[54].AddRange(new string[] { "DET_CAR", "BUF_FLA", "BOS_MTL", "WSH_OTT", "TBL_PIT", "TOR_PHI", "CBJ_NYR", "NJD_NYI", "EDM_CHI", "CGY_LAK", "ANA_SJS", "WPG_SEA", "VAN_UTA", "VGK_STL", "COL_NSH", "DAL_MIN" });
                gameStringsNhl[55].AddRange(new string[] { "WSH_CAR", "BOS_PIT", "BUF_PHI", "DET_NYR", "FLA_NYI", "MTL_NJD", "OTT_CBJ", "TBL_VGK", "VAN_TOR", "SEA_CHI", "SJS_COL", "LAK_DAL", "EDM_MIN", "CGY_NSH", "ANA_STL", "WPG_UTA" });
                gameStringsNhl[56].AddRange(new string[] { "CAR_VAN", "VGK_SEA", "CBJ_SJS", "NJD_LAK", "NYI_EDM", "NYR_CGY", "PHI_ANA", "PIT_WPG", "UTA_WSH", "STL_BOS", "NSH_BUF", "MIN_DET", "DAL_FLA", "COL_MTL", "CHI_OTT", "TOR_TBL" });
                gameStringsNhl[57].AddRange(new string[] { "BOS_CAR", "WSH_BUF", "PIT_DET", "PHI_FLA", "MTL_NYR", "OTT_NYI", "TBL_NJD", "TOR_CBJ", "ANA_CHI", "WPG_CGY", "UTA_EDM", "STL_LAK", "SJS_NSH", "SEA_MIN", "VAN_DAL", "VGK_COL" });
                gameStringsNhl[58].AddRange(new string[] { "COL_CAR", "CHI_DAL", "TOR_MIN", "TBL_NSH", "OTT_STL", "MTL_UTA", "FLA_WPG", "DET_ANA", "BUF_CGY", "EDM_BOS", "LAK_WSH", "SJS_PIT", "SEA_PHI", "VAN_NYR", "VGK_NYI", "CBJ_NJD" });
                gameStringsNhl[59].AddRange(new string[] { "CAR_NJD", "CBJ_NYI", "TOR_NYR", "TBL_PHI", "PIT_OTT", "WSH_MTL", "BOS_FLA", "BUF_DET", "CHI_DAL", "COL_MIN", "VGK_NSH", "VAN_STL", "UTA_SEA", "WPG_SJS", "ANA_LAK", "CGY_EDM" });
                gameStringsNhl[60].AddRange(new string[] { "CAR_COL", "DAL_CHI", "MIN_TOR", "NSH_TBL", "STL_OTT", "UTA_MTL", "WPG_FLA", "ANA_DET", "CGY_BUF", "BOS_EDM", "WSH_LAK", "PIT_SJS", "PHI_SEA", "NYR_VAN", "NYI_VGK", "NJD_CBJ" });
                gameStringsNhl[61].AddRange(new string[] { "FLA_CAR", "DET_MTL", "BUF_OTT", "BOS_TBL", "WSH_TOR", "CBJ_PIT", "NJD_PHI", "NYI_NYR", "LAK_CHI", "EDM_SJS", "CGY_SEA", "ANA_VAN", "WPG_VGK", "COL_UTA", "DAL_STL", "MIN_NSH" });
                gameStringsNhl[62].AddRange(new string[] { "STL_CAR", "NSH_UTA", "MIN_WPG", "DAL_ANA", "COL_CGY", "CHI_EDM", "TOR_LAK", "TBL_SJS", "OTT_SEA", "VAN_MTL", "VGK_FLA", "CBJ_DET", "NJD_BUF", "NYI_BOS", "NYR_WSH", "PHI_PIT" });
                gameStringsNhl[63].AddRange(new string[] { "PIT_CAR", "PHI_WSH", "NYR_CBJ", "NYI_NJD", "TBL_BOS", "OTT_TOR", "BUF_MTL", "FLA_DET", "UTA_CHI", "STL_WPG", "NSH_COL", "MIN_DAL", "VAN_ANA", "SEA_VGK", "CGY_SJS", "LAK_EDM" });
                gameStringsNhl[64].AddRange(new string[] { "PIT_CAR", "WSH_PHI", "BOS_NYR", "BUF_NYI", "DET_NJD", "FLA_CBJ", "MTL_VGK", "OTT_VAN", "SEA_TBL", "SJS_TOR", "LAK_CHI", "EDM_COL", "CGY_DAL", "ANA_MIN", "WPG_NSH", "UTA_STL" });
                gameStringsNhl[65].AddRange(new string[] { "CAR_PHI", "NYR_PIT", "WSH_NYI", "NJD_CBJ", "OTT_BOS", "MTL_TBL", "FLA_TOR", "BUF_DET", "CHI_STL", "NSH_UTA", "WPG_MIN", "DAL_COL", "SEA_ANA", "SJS_VAN", "LAK_VGK", "CGY_EDM" });
                gameStringsNhl[66].AddRange(new string[] { "NYI_CAR", "NYR_NJD", "PHI_CBJ", "PIT_VGK", "WSH_VAN", "BOS_SEA", "BUF_SJS", "DET_LAK", "EDM_FLA", "CGY_MTL", "ANA_OTT", "WPG_TBL", "UTA_TOR", "STL_CHI", "NSH_COL", "MIN_DAL" });
                gameStringsNhl[67].AddRange(new string[] { "MTL_CAR", "OTT_FLA", "TBL_DET", "TOR_BUF", "CHI_BOS", "COL_WSH", "DAL_PIT", "MIN_PHI", "NYR_NSH", "NYI_STL", "NJD_UTA", "CBJ_WPG", "VGK_ANA", "VAN_CGY", "SEA_EDM", "SJS_LAK" });
                gameStringsNhl[68].AddRange(new string[] { "CAR_LAK", "SJS_EDM", "SEA_CGY", "VAN_ANA", "VGK_WPG", "CBJ_UTA", "NJD_STL", "NYI_NSH", "MIN_NYR", "DAL_PHI", "COL_PIT", "CHI_WSH", "TOR_BOS", "TBL_BUF", "OTT_DET", "MTL_FLA" });
                gameStringsNhl[69].AddRange(new string[] { "TOR_CAR", "TBL_CBJ", "OTT_NJD", "MTL_NYI", "NYR_FLA", "PHI_DET", "PIT_BUF", "WSH_BOS", "VGK_CHI", "VAN_COL", "SEA_DAL", "SJS_MIN", "NSH_LAK", "STL_EDM", "UTA_CGY", "WPG_ANA" });
                gameStringsNhl[70].AddRange(new string[] { "CAR_CBJ", "TOR_NJD", "TBL_NYI", "OTT_NYR", "PHI_MTL", "PIT_FLA", "WSH_DET", "BOS_BUF", "CHI_COL", "VGK_DAL", "VAN_MIN", "SEA_NSH", "STL_SJS", "UTA_LAK", "WPG_EDM", "ANA_CGY" });
                gameStringsNhl[71].AddRange(new string[] { "NJD_CAR", "NYI_CBJ", "NYR_VGK", "PHI_VAN", "PIT_SEA", "WSH_SJS", "BOS_LAK", "BUF_EDM", "CGY_DET", "ANA_FLA", "WPG_MTL", "UTA_OTT", "STL_TBL", "NSH_TOR", "MIN_CHI", "DAL_COL" });
                gameStringsNhl[72].AddRange(new string[] { "CHI_CAR", "TOR_COL", "TBL_DAL", "OTT_MIN", "MTL_NSH", "FLA_STL", "DET_UTA", "BUF_WPG", "ANA_BOS", "CGY_WSH", "EDM_PIT", "LAK_PHI", "SJS_NYR", "SEA_NYI", "VAN_NJD", "VGK_CBJ" });
                gameStringsNhl[73].AddRange(new string[] { "CAR_EDM", "LAK_CGY", "SJS_ANA", "SEA_WPG", "VAN_UTA", "VGK_STL", "CBJ_NSH", "NJD_MIN", "DAL_NYI", "COL_NYR", "CHI_PHI", "TOR_PIT", "TBL_WSH", "OTT_BOS", "MTL_BUF", "FLA_DET" });
                gameStringsNhl[74].AddRange(new string[] { "BUF_CAR", "BOS_DET", "WSH_FLA", "PIT_MTL", "PHI_OTT", "TBL_NYR", "TOR_NYI", "CBJ_NJD", "CGY_CHI", "ANA_EDM", "WPG_LAK", "UTA_SJS", "STL_SEA", "VAN_NSH", "VGK_MIN", "COL_DAL" });
                gameStringsNhl[75].AddRange(new string[] { "CBJ_CAR", "NJD_VGK", "NYI_VAN", "NYR_SEA", "PHI_SJS", "PIT_LAK", "WSH_EDM", "BOS_CGY", "ANA_BUF", "WPG_DET", "UTA_FLA", "STL_MTL", "NSH_OTT", "MIN_TBL", "DAL_TOR", "COL_CHI" });
                gameStringsNhl[76].AddRange(new string[] { "CAR_SJS", "SEA_LAK", "VAN_EDM", "VGK_CGY", "CBJ_ANA", "NJD_WPG", "NYI_UTA", "NYR_STL", "NSH_PHI", "MIN_PIT", "DAL_WSH", "COL_BOS", "CHI_BUF", "TOR_DET", "TBL_FLA", "OTT_MTL" });
                gameStringsNhl[77].AddRange(new string[] { "TBL_CAR", "TOR_OTT", "CHI_MTL", "COL_FLA", "DAL_DET", "MIN_BUF", "NSH_BOS", "STL_WSH", "PIT_UTA", "PHI_WPG", "NYR_ANA", "NYI_CGY", "NJD_EDM", "CBJ_LAK", "VGK_SJS", "VAN_SEA" });
                gameStringsNhl[78].AddRange(new string[] { "BOS_CAR", "BUF_WSH", "DET_PIT", "FLA_PHI", "MTL_NYR", "OTT_NYI", "TBL_NJD", "TOR_CBJ", "VGK_CHI", "VAN_COL", "SEA_DAL", "SJS_MIN", "LAK_NSH", "EDM_STL", "CGY_UTA", "ANA_WPG" });
                gameStringsNhl[79].AddRange(new string[] { "VGK_CAR", "VAN_CBJ", "SEA_NJD", "SJS_NYI", "LAK_NYR", "EDM_PHI", "CGY_PIT", "ANA_WSH", "BOS_WPG", "BUF_UTA", "DET_STL", "FLA_NSH", "MTL_MIN", "OTT_DAL", "TBL_COL", "TOR_CHI" });
                gameStringsNhl[80].AddRange(new string[] { "CAR_CGY", "EDM_ANA", "LAK_WPG", "SJS_UTA", "SEA_STL", "VAN_NSH", "VGK_MIN", "CBJ_DAL", "COL_NJD", "CHI_NYI", "TOR_NYR", "TBL_PHI", "OTT_PIT", "MTL_WSH", "FLA_BOS", "DET_BUF" });
                gameStringsNhl[81].AddRange(new string[] { "CAR_WSH", "PIT_BOS", "PHI_BUF", "NYR_DET", "NYI_FLA", "MTL_NJD", "OTT_CBJ", "TBL_TOR", "CHI_WPG", "UTA_ANA", "STL_CGY", "NSH_EDM", "MIN_LAK", "SJS_DAL", "SEA_COL", "VAN_VGK" });
                gameStringsNhl[82].AddRange(new string[] { "MIN_CAR", "DAL_NSH", "COL_STL", "CHI_UTA", "TOR_WPG", "TBL_ANA", "OTT_CGY", "MTL_EDM", "FLA_LAK", "SJS_DET", "SEA_BUF", "VAN_BOS", "VGK_WSH", "CBJ_PIT", "NJD_PHI", "NYI_NYR" });
                gameStringsPwhl[1].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[2].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[3].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[4].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[5].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[6].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[7].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[8].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[9].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[10].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[11].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[12].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[13].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[14].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[15].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[16].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[17].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[18].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[19].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[20].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[21].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[22].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[23].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[24].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[25].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[26].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[27].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[28].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[29].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[30].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[31].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[32].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[33].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[34].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[35].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[36].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[37].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[38].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[39].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[40].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[41].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[42].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[43].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[44].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[45].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[46].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[47].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[48].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[49].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[50].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[51].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[52].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[53].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[54].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[55].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[56].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[57].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[58].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[59].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[60].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[61].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[62].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[63].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[64].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[65].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[66].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[67].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[68].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[69].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[70].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[71].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[72].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[73].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[74].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[75].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[76].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[77].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[78].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[79].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[80].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[81].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[82].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                break;
            case 3:
                gameStringsNhl[1].AddRange(new string[] { "CAR_MIN", "NSH_DAL", "STL_COL", "UTA_CHI", "WPG_TOR", "ANA_TBL", "CGY_OTT", "EDM_MTL", "LAK_FLA", "DET_SJS", "BUF_SEA", "BOS_VAN", "WSH_VGK", "PIT_CBJ", "PHI_NJD", "NYR_NYI" });
                gameStringsNhl[2].AddRange(new string[] { "NYI_CAR", "NYR_NJD", "PHI_CBJ", "PIT_VGK", "WSH_VAN", "BOS_SEA", "BUF_SJS", "DET_LAK", "EDM_FLA", "CGY_MTL", "ANA_OTT", "WPG_TBL", "UTA_TOR", "STL_CHI", "NSH_COL", "MIN_DAL" });
                gameStringsNhl[3].AddRange(new string[] { "CAR_CBJ", "TOR_NJD", "TBL_NYI", "OTT_NYR", "PHI_MTL", "PIT_FLA", "WSH_DET", "BOS_BUF", "CHI_COL", "VGK_DAL", "VAN_MIN", "SEA_NSH", "STL_SJS", "UTA_LAK", "WPG_EDM", "ANA_CGY" });
                gameStringsNhl[4].AddRange(new string[] { "TBL_CAR", "TOR_OTT", "CHI_MTL", "COL_FLA", "DAL_DET", "MIN_BUF", "NSH_BOS", "STL_WSH", "PIT_UTA", "PHI_WPG", "NYR_ANA", "NYI_CGY", "NJD_EDM", "CBJ_LAK", "VGK_SJS", "VAN_SEA" });
                gameStringsNhl[5].AddRange(new string[] { "BUF_CAR", "DET_BOS", "FLA_WSH", "MTL_PIT", "OTT_PHI", "TBL_NYR", "TOR_NYI", "CHI_NJD", "COL_CBJ", "VGK_DAL", "VAN_MIN", "SEA_NSH", "SJS_STL", "LAK_UTA", "EDM_WPG", "CGY_ANA" });
                gameStringsNhl[6].AddRange(new string[] { "CAR_EDM", "LAK_CGY", "SJS_ANA", "SEA_WPG", "VAN_UTA", "VGK_STL", "CBJ_NSH", "NJD_MIN", "DAL_NYI", "COL_NYR", "CHI_PHI", "TOR_PIT", "TBL_WSH", "OTT_BOS", "MTL_BUF", "FLA_DET" });
                gameStringsNhl[7].AddRange(new string[] { "PIT_CAR", "PHI_WSH", "NYR_CBJ", "NYI_NJD", "TBL_BOS", "OTT_TOR", "BUF_MTL", "FLA_DET", "UTA_CHI", "STL_WPG", "NSH_COL", "MIN_DAL", "VAN_ANA", "SEA_VGK", "CGY_SJS", "LAK_EDM" });
                gameStringsNhl[8].AddRange(new string[] { "MTL_CAR", "OTT_FLA", "TBL_DET", "TOR_BUF", "CHI_BOS", "COL_WSH", "DAL_PIT", "MIN_PHI", "NYR_NSH", "NYI_STL", "NJD_UTA", "CBJ_WPG", "VGK_ANA", "VAN_CGY", "SEA_EDM", "SJS_LAK" });
                gameStringsNhl[9].AddRange(new string[] { "CAR_PIT", "PHI_WSH", "NYR_BOS", "NYI_BUF", "DET_NJD", "FLA_CBJ", "MTL_TOR", "OTT_TBL", "CHI_UTA", "STL_WPG", "NSH_ANA", "MIN_CGY", "EDM_DAL", "LAK_COL", "SJS_VGK", "SEA_VAN" });
                gameStringsNhl[10].AddRange(new string[] { "CAR_DAL", "MIN_COL", "NSH_CHI", "STL_TOR", "UTA_TBL", "WPG_OTT", "ANA_MTL", "CGY_FLA", "DET_EDM", "BUF_LAK", "BOS_SJS", "WSH_SEA", "PIT_VAN", "PHI_VGK", "NYR_CBJ", "NYI_NJD" });
                gameStringsNhl[11].AddRange(new string[] { "CAR_MTL", "FLA_OTT", "DET_TBL", "BUF_TOR", "BOS_CHI", "WSH_COL", "PIT_DAL", "PHI_MIN", "NSH_NYR", "STL_NYI", "UTA_NJD", "WPG_CBJ", "ANA_VGK", "CGY_VAN", "EDM_SEA", "LAK_SJS" });
                gameStringsNhl[12].AddRange(new string[] { "CAR_LAK", "SJS_EDM", "SEA_CGY", "VAN_ANA", "VGK_WPG", "CBJ_UTA", "NJD_STL", "NYI_NSH", "MIN_NYR", "DAL_PHI", "COL_PIT", "CHI_WSH", "TOR_BOS", "TBL_BUF", "OTT_DET", "MTL_FLA" });
                gameStringsNhl[13].AddRange(new string[] { "CAR_NYR", "NYI_PHI", "NJD_PIT", "CBJ_WSH", "BOS_TOR", "BUF_TBL", "DET_OTT", "FLA_MTL", "CHI_NSH", "MIN_STL", "DAL_UTA", "COL_WPG", "ANA_VGK", "CGY_VAN", "EDM_SEA", "LAK_SJS" });
                gameStringsNhl[14].AddRange(new string[] { "CAR_OTT", "MTL_TBL", "FLA_TOR", "DET_CHI", "BUF_COL", "BOS_DAL", "WSH_MIN", "PIT_NSH", "PHI_STL", "UTA_NYR", "WPG_NYI", "ANA_NJD", "CGY_CBJ", "EDM_VGK", "LAK_VAN", "SJS_SEA" });
                gameStringsNhl[15].AddRange(new string[] { "ANA_CAR", "WPG_CGY", "UTA_EDM", "STL_LAK", "NSH_SJS", "MIN_SEA", "DAL_VAN", "COL_VGK", "CBJ_CHI", "NJD_TOR", "NYI_TBL", "NYR_OTT", "PHI_MTL", "PIT_FLA", "WSH_DET", "BOS_BUF" });
                gameStringsNhl[16].AddRange(new string[] { "CAR_BUF", "BOS_DET", "WSH_FLA", "PIT_MTL", "PHI_OTT", "NYR_TBL", "NYI_TOR", "NJD_CHI", "CBJ_COL", "DAL_VGK", "MIN_VAN", "NSH_SEA", "STL_SJS", "UTA_LAK", "WPG_EDM", "ANA_CGY" });
                gameStringsNhl[17].AddRange(new string[] { "CAR_NYR", "NYI_PHI", "NJD_PIT", "CBJ_WSH", "BOS_MTL", "FLA_OTT", "DET_TBL", "TOR_BUF", "CHI_NSH", "MIN_STL", "DAL_UTA", "COL_WPG", "ANA_SJS", "LAK_SEA", "EDM_VAN", "VGK_CGY" });
                gameStringsNhl[18].AddRange(new string[] { "CAR_NSH", "STL_MIN", "UTA_DAL", "WPG_COL", "ANA_CHI", "CGY_TOR", "EDM_TBL", "LAK_OTT", "MTL_SJS", "FLA_SEA", "DET_VAN", "BUF_VGK", "BOS_CBJ", "WSH_NJD", "PIT_NYI", "PHI_NYR" });
                gameStringsNhl[19].AddRange(new string[] { "CAR_NJD", "CBJ_NYI", "TOR_NYR", "TBL_PHI", "PIT_OTT", "WSH_MTL", "BOS_FLA", "BUF_DET", "CHI_DAL", "COL_MIN", "VGK_NSH", "VAN_STL", "UTA_SEA", "WPG_SJS", "ANA_LAK", "CGY_EDM" });
                gameStringsNhl[20].AddRange(new string[] { "NYR_CAR", "PHI_NYI", "PIT_NJD", "WSH_CBJ", "BOS_VGK", "BUF_VAN", "DET_SEA", "FLA_SJS", "LAK_MTL", "EDM_OTT", "CGY_TBL", "ANA_TOR", "WPG_CHI", "UTA_COL", "STL_DAL", "NSH_MIN" });
                gameStringsNhl[21].AddRange(new string[] { "NJD_CAR", "NYI_CBJ", "NYR_VGK", "PHI_VAN", "PIT_SEA", "WSH_SJS", "BOS_LAK", "BUF_EDM", "CGY_DET", "ANA_FLA", "WPG_MTL", "UTA_OTT", "STL_TBL", "NSH_TOR", "MIN_CHI", "DAL_COL" });
                gameStringsNhl[22].AddRange(new string[] { "SEA_CAR", "SJS_VAN", "LAK_VGK", "EDM_CBJ", "CGY_NJD", "ANA_NYI", "WPG_NYR", "UTA_PHI", "PIT_STL", "WSH_NSH", "BOS_MIN", "BUF_DAL", "DET_COL", "FLA_CHI", "MTL_TOR", "OTT_TBL" });
                gameStringsNhl[23].AddRange(new string[] { "CAR_VAN", "VGK_SEA", "CBJ_SJS", "NJD_LAK", "NYI_EDM", "NYR_CGY", "PHI_ANA", "PIT_WPG", "UTA_WSH", "STL_BOS", "NSH_BUF", "MIN_DET", "DAL_FLA", "COL_MTL", "CHI_OTT", "TOR_TBL" });
                gameStringsNhl[24].AddRange(new string[] { "PHI_CAR", "PIT_NYR", "WSH_NYI", "BOS_NJD", "BUF_CBJ", "DET_VGK", "FLA_VAN", "MTL_SEA", "SJS_OTT", "LAK_TBL", "EDM_TOR", "CGY_CHI", "ANA_COL", "WPG_DAL", "UTA_MIN", "STL_NSH" });
                gameStringsNhl[25].AddRange(new string[] { "CAR_NJD", "CBJ_NYI", "VGK_NYR", "VAN_PHI", "SEA_PIT", "SJS_WSH", "LAK_BOS", "EDM_BUF", "DET_CGY", "FLA_ANA", "MTL_WPG", "OTT_UTA", "TBL_STL", "TOR_NSH", "CHI_MIN", "COL_DAL" });
                gameStringsNhl[26].AddRange(new string[] { "OTT_CAR", "TBL_MTL", "TOR_FLA", "CHI_DET", "COL_BUF", "DAL_BOS", "MIN_WSH", "NSH_PIT", "STL_PHI", "NYR_UTA", "NYI_WPG", "NJD_ANA", "CBJ_CGY", "VGK_EDM", "VAN_LAK", "SEA_SJS" });
                gameStringsNhl[27].AddRange(new string[] { "CAR_FLA", "DET_MTL", "BUF_OTT", "BOS_TBL", "WSH_TOR", "PIT_CHI", "PHI_COL", "NYR_DAL", "NYI_MIN", "NSH_NJD", "STL_CBJ", "UTA_VGK", "WPG_VAN", "ANA_SEA", "CGY_SJS", "EDM_LAK" });
                gameStringsNhl[28].AddRange(new string[] { "WPG_CAR", "UTA_ANA", "STL_CGY", "NSH_EDM", "MIN_LAK", "DAL_SJS", "COL_SEA", "CHI_VAN", "TOR_VGK", "CBJ_TBL", "NJD_OTT", "NYI_MTL", "NYR_FLA", "PHI_DET", "PIT_BUF", "WSH_BOS" });
                gameStringsNhl[29].AddRange(new string[] { "CHI_CAR", "TOR_COL", "TBL_DAL", "OTT_MIN", "MTL_NSH", "FLA_STL", "DET_UTA", "BUF_WPG", "ANA_BOS", "CGY_WSH", "EDM_PIT", "LAK_PHI", "SJS_NYR", "SEA_NYI", "VAN_NJD", "VGK_CBJ" });
                gameStringsNhl[30].AddRange(new string[] { "CAR_COL", "DAL_CHI", "MIN_TOR", "NSH_TBL", "STL_OTT", "UTA_MTL", "WPG_FLA", "ANA_DET", "CGY_BUF", "BOS_EDM", "WSH_LAK", "PIT_SJS", "PHI_SEA", "NYR_VAN", "NYI_VGK", "NJD_CBJ" });
                gameStringsNhl[31].AddRange(new string[] { "VAN_CAR", "SEA_VGK", "SJS_CBJ", "LAK_NJD", "EDM_NYI", "CGY_NYR", "ANA_PHI", "WPG_PIT", "WSH_UTA", "BOS_STL", "BUF_NSH", "DET_MIN", "FLA_DAL", "MTL_COL", "OTT_CHI", "TBL_TOR" });
                gameStringsNhl[32].AddRange(new string[] { "CAR_ANA", "CGY_WPG", "EDM_UTA", "LAK_STL", "SJS_NSH", "SEA_MIN", "VAN_DAL", "VGK_COL", "CHI_CBJ", "TOR_NJD", "TBL_NYI", "OTT_NYR", "MTL_PHI", "FLA_PIT", "DET_WSH", "BUF_BOS" });
                gameStringsNhl[33].AddRange(new string[] { "LAK_CAR", "EDM_SJS", "CGY_SEA", "ANA_VAN", "WPG_VGK", "UTA_CBJ", "STL_NJD", "NSH_NYI", "NYR_MIN", "PHI_DAL", "PIT_COL", "WSH_CHI", "BOS_TOR", "BUF_TBL", "DET_OTT", "FLA_MTL" });
                gameStringsNhl[34].AddRange(new string[] { "CAR_NYI", "NJD_NYR", "CBJ_PHI", "VGK_PIT", "VAN_WSH", "SEA_BOS", "SJS_BUF", "LAK_DET", "FLA_EDM", "MTL_CGY", "OTT_ANA", "TBL_WPG", "TOR_UTA", "CHI_STL", "COL_NSH", "DAL_MIN" });
                gameStringsNhl[35].AddRange(new string[] { "CAR_WSH", "PIT_BOS", "PHI_BUF", "NYR_DET", "NYI_FLA", "NJD_MTL", "CBJ_OTT", "VGK_TBL", "TOR_VAN", "CHI_SEA", "COL_SJS", "DAL_LAK", "MIN_EDM", "NSH_CGY", "STL_ANA", "UTA_WPG" });
                gameStringsNhl[36].AddRange(new string[] { "CAR_CGY", "EDM_ANA", "LAK_WPG", "SJS_UTA", "SEA_STL", "VAN_NSH", "VGK_MIN", "CBJ_DAL", "COL_NJD", "CHI_NYI", "TOR_NYR", "TBL_PHI", "OTT_PIT", "MTL_WSH", "FLA_BOS", "DET_BUF" });
                gameStringsNhl[37].AddRange(new string[] { "STL_CAR", "NSH_UTA", "MIN_WPG", "DAL_ANA", "COL_CGY", "CHI_EDM", "TOR_LAK", "TBL_SJS", "OTT_SEA", "VAN_MTL", "VGK_FLA", "CBJ_DET", "NJD_BUF", "NYI_BOS", "NYR_WSH", "PHI_PIT" });
                gameStringsNhl[38].AddRange(new string[] { "CAR_STL", "UTA_NSH", "WPG_MIN", "ANA_DAL", "CGY_COL", "EDM_CHI", "LAK_TOR", "SJS_TBL", "SEA_OTT", "MTL_VAN", "FLA_VGK", "DET_CBJ", "BUF_NJD", "BOS_NYI", "WSH_NYR", "PIT_PHI" });
                gameStringsNhl[39].AddRange(new string[] { "CAR_VGK", "CBJ_VAN", "NJD_SEA", "NYI_SJS", "NYR_LAK", "PHI_EDM", "PIT_CGY", "WSH_ANA", "WPG_BOS", "UTA_BUF", "STL_DET", "NSH_FLA", "MIN_MTL", "DAL_OTT", "COL_TBL", "CHI_TOR" });
                gameStringsNhl[40].AddRange(new string[] { "CAR_CBJ", "VGK_NJD", "VAN_NYI", "SEA_NYR", "SJS_PHI", "LAK_PIT", "EDM_WSH", "CGY_BOS", "BUF_ANA", "DET_WPG", "FLA_UTA", "MTL_STL", "OTT_NSH", "TBL_MIN", "TOR_DAL", "CHI_COL" });
                gameStringsNhl[41].AddRange(new string[] { "FLA_CAR", "DET_MTL", "BUF_OTT", "BOS_TBL", "WSH_TOR", "CBJ_PIT", "NJD_PHI", "NYI_NYR", "LAK_CHI", "EDM_SJS", "CGY_SEA", "ANA_VAN", "WPG_VGK", "COL_UTA", "DAL_STL", "MIN_NSH" });
                gameStringsNhl[42].AddRange(new string[] { "CAR_WPG", "ANA_UTA", "CGY_STL", "EDM_NSH", "LAK_MIN", "SJS_DAL", "SEA_COL", "VAN_CHI", "VGK_TOR", "TBL_CBJ", "OTT_NJD", "MTL_NYI", "FLA_NYR", "DET_PHI", "BUF_PIT", "BOS_WSH" });
                gameStringsNhl[43].AddRange(new string[] { "CAR_PHI", "NYR_PIT", "NYI_WSH", "NJD_BOS", "CBJ_BUF", "VGK_DET", "VAN_FLA", "SEA_MTL", "OTT_SJS", "TBL_LAK", "TOR_EDM", "CHI_CGY", "COL_ANA", "DAL_WPG", "MIN_UTA", "NSH_STL" });
                gameStringsNhl[44].AddRange(new string[] { "CAR_SEA", "VAN_SJS", "VGK_LAK", "CBJ_EDM", "NJD_CGY", "NYI_ANA", "NYR_WPG", "PHI_UTA", "STL_PIT", "NSH_WSH", "MIN_BOS", "DAL_BUF", "COL_DET", "CHI_FLA", "TOR_MTL", "TBL_OTT" });
                gameStringsNhl[45].AddRange(new string[] { "VGK_CAR", "VAN_CBJ", "SEA_NJD", "SJS_NYI", "LAK_NYR", "EDM_PHI", "CGY_PIT", "ANA_WSH", "BOS_WPG", "BUF_UTA", "DET_STL", "FLA_NSH", "MTL_MIN", "OTT_DAL", "TBL_COL", "TOR_CHI" });
                gameStringsNhl[46].AddRange(new string[] { "CAR_CHI", "COL_TOR", "DAL_TBL", "MIN_OTT", "NSH_MTL", "STL_FLA", "UTA_DET", "WPG_BUF", "BOS_ANA", "WSH_CGY", "PIT_EDM", "PHI_LAK", "NYR_SJS", "NYI_SEA", "NJD_VAN", "CBJ_VGK" });
                gameStringsNhl[47].AddRange(new string[] { "CAR_NYI", "NJD_NYR", "CBJ_PHI", "TOR_PIT", "WSH_TBL", "BOS_OTT", "BUF_MTL", "DET_FLA", "CHI_MIN", "DAL_NSH", "COL_STL", "VGK_UTA", "WPG_VAN", "ANA_SEA", "CGY_SJS", "EDM_LAK" });
                gameStringsNhl[48].AddRange(new string[] { "TOR_CAR", "CHI_TBL", "COL_OTT", "DAL_MTL", "MIN_FLA", "NSH_DET", "STL_BUF", "UTA_BOS", "WPG_WSH", "PIT_ANA", "PHI_CGY", "NYR_EDM", "NYI_LAK", "NJD_SJS", "CBJ_SEA", "VGK_VAN" });
                gameStringsNhl[49].AddRange(new string[] { "OTT_CAR", "MTL_TBL", "FLA_TOR", "DET_CBJ", "NJD_BUF", "NYI_BOS", "NYR_WSH", "PHI_PIT", "SEA_CHI", "SJS_VAN", "LAK_VGK", "EDM_COL", "DAL_CGY", "MIN_ANA", "NSH_WPG", "STL_UTA" });
                gameStringsNhl[50].AddRange(new string[] { "SJS_CAR", "LAK_SEA", "EDM_VAN", "CGY_VGK", "ANA_CBJ", "WPG_NJD", "UTA_NYI", "STL_NYR", "PHI_NSH", "PIT_MIN", "WSH_DAL", "BOS_COL", "BUF_CHI", "DET_TOR", "FLA_TBL", "MTL_OTT" });
                gameStringsNhl[51].AddRange(new string[] { "PIT_CAR", "WSH_PHI", "BOS_NYR", "BUF_NYI", "DET_NJD", "FLA_CBJ", "MTL_VGK", "OTT_VAN", "SEA_TBL", "SJS_TOR", "LAK_CHI", "EDM_COL", "CGY_DAL", "ANA_MIN", "WPG_NSH", "UTA_STL" });
                gameStringsNhl[52].AddRange(new string[] { "COL_CAR", "CHI_DAL", "TOR_MIN", "TBL_NSH", "OTT_STL", "MTL_UTA", "FLA_WPG", "DET_ANA", "BUF_CGY", "EDM_BOS", "LAK_WSH", "SJS_PIT", "SEA_PHI", "VAN_NYR", "VGK_NYI", "CBJ_NJD" });
                gameStringsNhl[53].AddRange(new string[] { "MTL_CAR", "FLA_OTT", "DET_TBL", "BUF_TOR", "CBJ_BOS", "NJD_WSH", "NYI_PIT", "NYR_PHI", "SJS_CHI", "LAK_SEA", "EDM_VAN", "CGY_VGK", "COL_ANA", "DAL_WPG", "MIN_UTA", "NSH_STL" });
                gameStringsNhl[54].AddRange(new string[] { "BUF_CAR", "BOS_DET", "WSH_FLA", "PIT_MTL", "PHI_OTT", "TBL_NYR", "TOR_NYI", "CBJ_NJD", "CGY_CHI", "ANA_EDM", "WPG_LAK", "UTA_SJS", "STL_SEA", "VAN_NSH", "VGK_MIN", "COL_DAL" });
                gameStringsNhl[55].AddRange(new string[] { "NSH_CAR", "MIN_STL", "DAL_UTA", "COL_WPG", "CHI_ANA", "TOR_CGY", "TBL_EDM", "OTT_LAK", "SJS_MTL", "SEA_FLA", "VAN_DET", "VGK_BUF", "CBJ_BOS", "NJD_WSH", "NYI_PIT", "NYR_PHI" });
                gameStringsNhl[56].AddRange(new string[] { "WSH_CAR", "PIT_CBJ", "PHI_NJD", "NYR_NYI", "TOR_BOS", "TBL_BUF", "OTT_DET", "MTL_FLA", "WPG_CHI", "UTA_COL", "STL_DAL", "NSH_MIN", "VGK_ANA", "VAN_CGY", "SEA_EDM", "SJS_LAK" });
                gameStringsNhl[57].AddRange(new string[] { "FLA_CAR", "MTL_DET", "OTT_BUF", "TBL_BOS", "TOR_WSH", "CHI_PIT", "COL_PHI", "DAL_NYR", "MIN_NYI", "NJD_NSH", "CBJ_STL", "VGK_UTA", "VAN_WPG", "SEA_ANA", "SJS_CGY", "LAK_EDM" });
                gameStringsNhl[58].AddRange(new string[] { "MIN_CAR", "DAL_NSH", "COL_STL", "CHI_UTA", "TOR_WPG", "TBL_ANA", "OTT_CGY", "MTL_EDM", "FLA_LAK", "SJS_DET", "SEA_BUF", "VAN_BOS", "VGK_WSH", "CBJ_PIT", "NJD_PHI", "NYI_NYR" });
                gameStringsNhl[59].AddRange(new string[] { "DET_CAR", "FLA_BUF", "MTL_BOS", "OTT_WSH", "TBL_PIT", "TOR_PHI", "CHI_NYR", "COL_NYI", "NJD_DAL", "CBJ_MIN", "VGK_NSH", "VAN_STL", "SEA_UTA", "SJS_WPG", "LAK_ANA", "EDM_CGY" });
                gameStringsNhl[60].AddRange(new string[] { "UTA_CAR", "STL_WPG", "NSH_ANA", "MIN_CGY", "DAL_EDM", "COL_LAK", "CHI_SJS", "TOR_SEA", "VAN_TBL", "VGK_OTT", "CBJ_MTL", "NJD_FLA", "NYI_DET", "NYR_BUF", "PHI_BOS", "PIT_WSH" });
                gameStringsNhl[61].AddRange(new string[] { "CAR_TOR", "TBL_CHI", "OTT_COL", "MTL_DAL", "FLA_MIN", "DET_NSH", "BUF_STL", "BOS_UTA", "WSH_WPG", "ANA_PIT", "CGY_PHI", "EDM_NYR", "LAK_NYI", "SJS_NJD", "SEA_CBJ", "VAN_VGK" });
                gameStringsNhl[62].AddRange(new string[] { "DAL_CAR", "COL_MIN", "CHI_NSH", "TOR_STL", "TBL_UTA", "OTT_WPG", "MTL_ANA", "FLA_CGY", "EDM_DET", "LAK_BUF", "SJS_BOS", "SEA_WSH", "VAN_PIT", "VGK_PHI", "CBJ_NYR", "NJD_NYI" });
                gameStringsNhl[63].AddRange(new string[] { "DET_CAR", "BUF_FLA", "BOS_MTL", "WSH_OTT", "TBL_PIT", "TOR_PHI", "CBJ_NYR", "NJD_NYI", "EDM_CHI", "CGY_LAK", "ANA_SJS", "WPG_SEA", "VAN_UTA", "VGK_STL", "COL_NSH", "DAL_MIN" });
                gameStringsNhl[64].AddRange(new string[] { "CAR_NYR", "NYI_PHI", "NJD_PIT", "CBJ_WSH", "VGK_BOS", "VAN_BUF", "SEA_DET", "SJS_FLA", "MTL_LAK", "OTT_EDM", "TBL_CGY", "TOR_ANA", "CHI_WPG", "COL_UTA", "DAL_STL", "MIN_NSH" });
                gameStringsNhl[65].AddRange(new string[] { "CGY_CAR", "ANA_EDM", "WPG_LAK", "UTA_SJS", "STL_SEA", "NSH_VAN", "MIN_VGK", "DAL_CBJ", "NJD_COL", "NYI_CHI", "NYR_TOR", "PHI_TBL", "PIT_OTT", "WSH_MTL", "BOS_FLA", "BUF_DET" });
                gameStringsNhl[66].AddRange(new string[] { "CAR_PHI", "NYR_PIT", "WSH_NYI", "NJD_CBJ", "OTT_BOS", "MTL_TBL", "FLA_TOR", "BUF_DET", "CHI_STL", "NSH_UTA", "WPG_MIN", "DAL_COL", "SEA_ANA", "SJS_VAN", "LAK_VGK", "CGY_EDM" });
                gameStringsNhl[67].AddRange(new string[] { "CAR_SJS", "SEA_LAK", "VAN_EDM", "VGK_CGY", "CBJ_ANA", "NJD_WPG", "NYI_UTA", "NYR_STL", "NSH_PHI", "MIN_PIT", "DAL_WSH", "COL_BOS", "CHI_BUF", "TOR_DET", "TBL_FLA", "OTT_MTL" });
                gameStringsNhl[68].AddRange(new string[] { "CAR_WSH", "PIT_BOS", "PHI_BUF", "NYR_DET", "NYI_FLA", "MTL_NJD", "OTT_CBJ", "TBL_TOR", "CHI_WPG", "UTA_ANA", "STL_CGY", "NSH_EDM", "MIN_LAK", "SJS_DAL", "SEA_COL", "VAN_VGK" });
                gameStringsNhl[69].AddRange(new string[] { "BOS_CAR", "WSH_BUF", "PIT_DET", "PHI_FLA", "MTL_NYR", "OTT_NYI", "TBL_NJD", "TOR_CBJ", "ANA_CHI", "WPG_CGY", "UTA_EDM", "STL_LAK", "SJS_NSH", "SEA_MIN", "VAN_DAL", "VGK_COL" });
                gameStringsNhl[70].AddRange(new string[] { "TOR_CAR", "TBL_CBJ", "OTT_NJD", "MTL_NYI", "NYR_FLA", "PHI_DET", "PIT_BUF", "WSH_BOS", "VGK_CHI", "VAN_COL", "SEA_DAL", "SJS_MIN", "NSH_LAK", "STL_EDM", "UTA_CGY", "WPG_ANA" });
                gameStringsNhl[71].AddRange(new string[] { "CAR_NYI", "NJD_NYR", "CBJ_PHI", "PIT_WSH", "BOS_FLA", "DET_MTL", "BUF_OTT", "TOR_TBL", "CHI_MIN", "DAL_NSH", "COL_STL", "UTA_WPG", "ANA_LAK", "EDM_SJS", "CGY_SEA", "VGK_VAN" });
                gameStringsNhl[72].AddRange(new string[] { "EDM_CAR", "CGY_LAK", "ANA_SJS", "WPG_SEA", "UTA_VAN", "STL_VGK", "NSH_CBJ", "MIN_NJD", "NYI_DAL", "NYR_COL", "PHI_CHI", "PIT_TOR", "WSH_TBL", "BOS_OTT", "BUF_MTL", "DET_FLA" });
                gameStringsNhl[73].AddRange(new string[] { "CAR_BOS", "WSH_BUF", "PIT_DET", "PHI_FLA", "NYR_MTL", "NYI_OTT", "NJD_TBL", "CBJ_TOR", "CHI_VGK", "COL_VAN", "DAL_SEA", "MIN_SJS", "NSH_LAK", "STL_EDM", "UTA_CGY", "WPG_ANA" });
                gameStringsNhl[74].AddRange(new string[] { "CAR_UTA", "WPG_STL", "ANA_NSH", "CGY_MIN", "EDM_DAL", "LAK_COL", "SJS_CHI", "SEA_TOR", "TBL_VAN", "OTT_VGK", "MTL_CBJ", "FLA_NJD", "DET_NYI", "BUF_NYR", "BOS_PHI", "WSH_PIT" });
                gameStringsNhl[75].AddRange(new string[] { "CAR_DET", "BUF_FLA", "BOS_MTL", "WSH_OTT", "PIT_TBL", "PHI_TOR", "NYR_CHI", "NYI_COL", "DAL_NJD", "MIN_CBJ", "NSH_VGK", "STL_VAN", "UTA_SEA", "WPG_SJS", "ANA_LAK", "CGY_EDM" });
                gameStringsNhl[76].AddRange(new string[] { "WSH_CAR", "BOS_PIT", "BUF_PHI", "DET_NYR", "FLA_NYI", "MTL_NJD", "OTT_CBJ", "TBL_VGK", "VAN_TOR", "SEA_CHI", "SJS_COL", "LAK_DAL", "EDM_MIN", "CGY_NSH", "ANA_STL", "WPG_UTA" });
                gameStringsNhl[77].AddRange(new string[] { "CBJ_CAR", "NJD_VGK", "NYI_VAN", "NYR_SEA", "PHI_SJS", "PIT_LAK", "WSH_EDM", "BOS_CGY", "ANA_BUF", "WPG_DET", "UTA_FLA", "STL_MTL", "NSH_OTT", "MIN_TBL", "DAL_TOR", "COL_CHI" });
                gameStringsNhl[78].AddRange(new string[] { "BOS_CAR", "BUF_WSH", "DET_PIT", "FLA_PHI", "MTL_NYR", "OTT_NYI", "TBL_NJD", "TOR_CBJ", "VGK_CHI", "VAN_COL", "SEA_DAL", "SJS_MIN", "LAK_NSH", "EDM_STL", "CGY_UTA", "ANA_WPG" });
                gameStringsNhl[79].AddRange(new string[] { "CAR_PHI", "NYR_PIT", "NYI_WSH", "NJD_BOS", "CBJ_BUF", "DET_TOR", "FLA_TBL", "MTL_OTT", "CHI_STL", "NSH_UTA", "MIN_WPG", "DAL_ANA", "COL_CGY", "EDM_VGK", "LAK_VAN", "SJS_SEA" });
                gameStringsNhl[80].AddRange(new string[] { "CAR_TBL", "OTT_TOR", "MTL_CHI", "FLA_COL", "DET_DAL", "BUF_MIN", "BOS_NSH", "WSH_STL", "UTA_PIT", "WPG_PHI", "ANA_NYR", "CGY_NYI", "EDM_NJD", "LAK_CBJ", "SJS_VGK", "SEA_VAN" });
                gameStringsNhl[81].AddRange(new string[] { "TBL_CAR", "OTT_TOR", "MTL_CBJ", "FLA_NJD", "NYI_DET", "NYR_BUF", "PHI_BOS", "PIT_WSH", "VAN_CHI", "SEA_VGK", "SJS_COL", "LAK_DAL", "MIN_EDM", "NSH_CGY", "STL_ANA", "UTA_WPG" });
                gameStringsNhl[82].AddRange(new string[] { "CAR_PIT", "PHI_WSH", "NYR_BOS", "NYI_BUF", "NJD_DET", "CBJ_FLA", "VGK_MTL", "VAN_OTT", "TBL_SEA", "TOR_SJS", "CHI_LAK", "COL_EDM", "DAL_CGY", "MIN_ANA", "NSH_WPG", "STL_UTA" });
                gameStringsPwhl[1].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[2].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[3].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[4].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[5].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[6].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[7].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[8].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[9].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[10].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[11].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[12].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[13].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[14].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[15].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[16].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[17].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[18].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[19].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[20].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[21].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[22].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[23].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[24].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[25].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[26].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[27].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[28].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[29].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[30].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[31].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[32].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[33].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[34].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[35].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[36].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[37].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[38].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[39].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[40].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[41].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[42].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[43].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[44].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[45].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[46].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[47].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[48].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[49].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[50].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[51].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[52].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[53].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[54].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[55].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[56].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[57].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[58].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[59].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[60].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[61].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[62].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[63].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[64].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[65].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[66].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[67].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[68].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[69].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[70].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[71].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[72].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[73].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[74].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[75].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[76].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[77].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[78].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[79].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[80].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[81].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[82].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                break;
            case 1:
            default:
                gameStringsNhl[1].AddRange(new string[] { "CAR_NYR", "NYI_PHI", "NJD_PIT", "CBJ_WSH", "VGK_BOS", "VAN_BUF", "SEA_DET", "SJS_FLA", "MTL_LAK", "OTT_EDM", "TBL_CGY", "TOR_ANA", "CHI_WPG", "COL_UTA", "DAL_STL", "MIN_NSH" });
                gameStringsNhl[2].AddRange(new string[] { "OTT_CAR", "MTL_TBL", "FLA_TOR", "DET_CBJ", "NJD_BUF", "NYI_BOS", "NYR_WSH", "PHI_PIT", "SEA_CHI", "SJS_VAN", "LAK_VGK", "EDM_COL", "DAL_CGY", "MIN_ANA", "NSH_WPG", "STL_UTA" });
                gameStringsNhl[3].AddRange(new string[] { "LAK_CAR", "EDM_SJS", "CGY_SEA", "ANA_VAN", "WPG_VGK", "UTA_CBJ", "STL_NJD", "NSH_NYI", "NYR_MIN", "PHI_DAL", "PIT_COL", "WSH_CHI", "BOS_TOR", "BUF_TBL", "DET_OTT", "FLA_MTL" });
                gameStringsNhl[4].AddRange(new string[] { "CAR_NYR", "NYI_PHI", "NJD_PIT", "CBJ_WSH", "BOS_MTL", "FLA_OTT", "DET_TBL", "TOR_BUF", "CHI_NSH", "MIN_STL", "DAL_UTA", "COL_WPG", "ANA_SJS", "LAK_SEA", "EDM_VAN", "VGK_CGY" });
                gameStringsNhl[5].AddRange(new string[] { "TOR_CAR", "TBL_CBJ", "OTT_NJD", "MTL_NYI", "NYR_FLA", "PHI_DET", "PIT_BUF", "WSH_BOS", "VGK_CHI", "VAN_COL", "SEA_DAL", "SJS_MIN", "NSH_LAK", "STL_EDM", "UTA_CGY", "WPG_ANA" });
                gameStringsNhl[6].AddRange(new string[] { "CAR_MTL", "FLA_OTT", "DET_TBL", "BUF_TOR", "BOS_CHI", "WSH_COL", "PIT_DAL", "PHI_MIN", "NSH_NYR", "STL_NYI", "UTA_NJD", "WPG_CBJ", "ANA_VGK", "CGY_VAN", "EDM_SEA", "LAK_SJS" });
                gameStringsNhl[7].AddRange(new string[] { "CAR_COL", "DAL_CHI", "MIN_TOR", "NSH_TBL", "STL_OTT", "UTA_MTL", "WPG_FLA", "ANA_DET", "CGY_BUF", "BOS_EDM", "WSH_LAK", "PIT_SJS", "PHI_SEA", "NYR_VAN", "NYI_VGK", "NJD_CBJ" });
                gameStringsNhl[8].AddRange(new string[] { "CAR_BUF", "BOS_DET", "WSH_FLA", "PIT_MTL", "PHI_OTT", "NYR_TBL", "NYI_TOR", "NJD_CHI", "CBJ_COL", "DAL_VGK", "MIN_VAN", "NSH_SEA", "STL_SJS", "UTA_LAK", "WPG_EDM", "ANA_CGY" });
                gameStringsNhl[9].AddRange(new string[] { "CAR_VGK", "CBJ_VAN", "NJD_SEA", "NYI_SJS", "NYR_LAK", "PHI_EDM", "PIT_CGY", "WSH_ANA", "WPG_BOS", "UTA_BUF", "STL_DET", "NSH_FLA", "MIN_MTL", "DAL_OTT", "COL_TBL", "CHI_TOR" });
                gameStringsNhl[10].AddRange(new string[] { "CAR_ANA", "CGY_WPG", "EDM_UTA", "LAK_STL", "SJS_NSH", "SEA_MIN", "VAN_DAL", "VGK_COL", "CHI_CBJ", "TOR_NJD", "TBL_NYI", "OTT_NYR", "MTL_PHI", "FLA_PIT", "DET_WSH", "BUF_BOS" });
                gameStringsNhl[11].AddRange(new string[] { "CAR_SEA", "VAN_SJS", "VGK_LAK", "CBJ_EDM", "NJD_CGY", "NYI_ANA", "NYR_WPG", "PHI_UTA", "STL_PIT", "NSH_WSH", "MIN_BOS", "DAL_BUF", "COL_DET", "CHI_FLA", "TOR_MTL", "TBL_OTT" });
                gameStringsNhl[12].AddRange(new string[] { "EDM_CAR", "CGY_LAK", "ANA_SJS", "WPG_SEA", "UTA_VAN", "STL_VGK", "NSH_CBJ", "MIN_NJD", "NYI_DAL", "NYR_COL", "PHI_CHI", "PIT_TOR", "WSH_TBL", "BOS_OTT", "BUF_MTL", "DET_FLA" });
                gameStringsNhl[13].AddRange(new string[] { "CAR_TOR", "TBL_CHI", "OTT_COL", "MTL_DAL", "FLA_MIN", "DET_NSH", "BUF_STL", "BOS_UTA", "WSH_WPG", "ANA_PIT", "CGY_PHI", "EDM_NYR", "LAK_NYI", "SJS_NJD", "SEA_CBJ", "VAN_VGK" });
                gameStringsNhl[14].AddRange(new string[] { "CGY_CAR", "ANA_EDM", "WPG_LAK", "UTA_SJS", "STL_SEA", "NSH_VAN", "MIN_VGK", "DAL_CBJ", "NJD_COL", "NYI_CHI", "NYR_TOR", "PHI_TBL", "PIT_OTT", "WSH_MTL", "BOS_FLA", "BUF_DET" });
                gameStringsNhl[15].AddRange(new string[] { "WSH_CAR", "PIT_CBJ", "PHI_NJD", "NYR_NYI", "TOR_BOS", "TBL_BUF", "OTT_DET", "MTL_FLA", "WPG_CHI", "UTA_COL", "STL_DAL", "NSH_MIN", "VGK_ANA", "VAN_CGY", "SEA_EDM", "SJS_LAK" });
                gameStringsNhl[16].AddRange(new string[] { "CAR_NYI", "NJD_NYR", "CBJ_PHI", "VGK_PIT", "VAN_WSH", "SEA_BOS", "SJS_BUF", "LAK_DET", "FLA_EDM", "MTL_CGY", "OTT_ANA", "TBL_WPG", "TOR_UTA", "CHI_STL", "COL_NSH", "DAL_MIN" });
                gameStringsNhl[17].AddRange(new string[] { "CAR_WPG", "ANA_UTA", "CGY_STL", "EDM_NSH", "LAK_MIN", "SJS_DAL", "SEA_COL", "VAN_CHI", "VGK_TOR", "TBL_CBJ", "OTT_NJD", "MTL_NYI", "FLA_NYR", "DET_PHI", "BUF_PIT", "BOS_WSH" });
                gameStringsNhl[18].AddRange(new string[] { "PIT_CAR", "WSH_PHI", "BOS_NYR", "BUF_NYI", "DET_NJD", "FLA_CBJ", "MTL_VGK", "OTT_VAN", "SEA_TBL", "SJS_TOR", "LAK_CHI", "EDM_COL", "CGY_DAL", "ANA_MIN", "WPG_NSH", "UTA_STL" });
                gameStringsNhl[19].AddRange(new string[] { "BUF_CAR", "DET_BOS", "FLA_WSH", "MTL_PIT", "OTT_PHI", "TBL_NYR", "TOR_NYI", "CHI_NJD", "COL_CBJ", "VGK_DAL", "VAN_MIN", "SEA_NSH", "SJS_STL", "LAK_UTA", "EDM_WPG", "CGY_ANA" });
                gameStringsNhl[20].AddRange(new string[] { "CAR_CBJ", "TOR_NJD", "TBL_NYI", "OTT_NYR", "PHI_MTL", "PIT_FLA", "WSH_DET", "BOS_BUF", "CHI_COL", "VGK_DAL", "VAN_MIN", "SEA_NSH", "STL_SJS", "UTA_LAK", "WPG_EDM", "ANA_CGY" });
                gameStringsNhl[21].AddRange(new string[] { "PIT_CAR", "PHI_WSH", "NYR_CBJ", "NYI_NJD", "TBL_BOS", "OTT_TOR", "BUF_MTL", "FLA_DET", "UTA_CHI", "STL_WPG", "NSH_COL", "MIN_DAL", "VAN_ANA", "SEA_VGK", "CGY_SJS", "LAK_EDM" });
                gameStringsNhl[22].AddRange(new string[] { "CAR_DAL", "MIN_COL", "NSH_CHI", "STL_TOR", "UTA_TBL", "WPG_OTT", "ANA_MTL", "CGY_FLA", "DET_EDM", "BUF_LAK", "BOS_SJS", "WSH_SEA", "PIT_VAN", "PHI_VGK", "NYR_CBJ", "NYI_NJD" });
                gameStringsNhl[23].AddRange(new string[] { "CAR_WSH", "PIT_BOS", "PHI_BUF", "NYR_DET", "NYI_FLA", "NJD_MTL", "CBJ_OTT", "VGK_TBL", "TOR_VAN", "CHI_SEA", "COL_SJS", "DAL_LAK", "MIN_EDM", "NSH_CGY", "STL_ANA", "UTA_WPG" });
                gameStringsNhl[24].AddRange(new string[] { "CAR_CHI", "COL_TOR", "DAL_TBL", "MIN_OTT", "NSH_MTL", "STL_FLA", "UTA_DET", "WPG_BUF", "BOS_ANA", "WSH_CGY", "PIT_EDM", "PHI_LAK", "NYR_SJS", "NYI_SEA", "NJD_VAN", "CBJ_VGK" });
                gameStringsNhl[25].AddRange(new string[] { "CAR_NYI", "NJD_NYR", "CBJ_PHI", "PIT_WSH", "BOS_FLA", "DET_MTL", "BUF_OTT", "TOR_TBL", "CHI_MIN", "DAL_NSH", "COL_STL", "UTA_WPG", "ANA_LAK", "EDM_SJS", "CGY_SEA", "VGK_VAN" });
                gameStringsNhl[26].AddRange(new string[] { "CAR_PHI", "NYR_PIT", "WSH_NYI", "NJD_CBJ", "OTT_BOS", "MTL_TBL", "FLA_TOR", "BUF_DET", "CHI_STL", "NSH_UTA", "WPG_MIN", "DAL_COL", "SEA_ANA", "SJS_VAN", "LAK_VGK", "CGY_EDM" });
                gameStringsNhl[27].AddRange(new string[] { "CAR_PIT", "PHI_WSH", "NYR_BOS", "NYI_BUF", "NJD_DET", "CBJ_FLA", "VGK_MTL", "VAN_OTT", "TBL_SEA", "TOR_SJS", "CHI_LAK", "COL_EDM", "DAL_CGY", "MIN_ANA", "NSH_WPG", "STL_UTA" });
                gameStringsNhl[28].AddRange(new string[] { "CBJ_CAR", "NJD_VGK", "NYI_VAN", "NYR_SEA", "PHI_SJS", "PIT_LAK", "WSH_EDM", "BOS_CGY", "ANA_BUF", "WPG_DET", "UTA_FLA", "STL_MTL", "NSH_OTT", "MIN_TBL", "DAL_TOR", "COL_CHI" });
                gameStringsNhl[29].AddRange(new string[] { "CAR_BOS", "WSH_BUF", "PIT_DET", "PHI_FLA", "NYR_MTL", "NYI_OTT", "NJD_TBL", "CBJ_TOR", "CHI_VGK", "COL_VAN", "DAL_SEA", "MIN_SJS", "NSH_LAK", "STL_EDM", "UTA_CGY", "WPG_ANA" });
                gameStringsNhl[30].AddRange(new string[] { "CAR_LAK", "SJS_EDM", "SEA_CGY", "VAN_ANA", "VGK_WPG", "CBJ_UTA", "NJD_STL", "NYI_NSH", "MIN_NYR", "DAL_PHI", "COL_PIT", "CHI_WSH", "TOR_BOS", "TBL_BUF", "OTT_DET", "MTL_FLA" });
                gameStringsNhl[31].AddRange(new string[] { "DET_CAR", "FLA_BUF", "MTL_BOS", "OTT_WSH", "TBL_PIT", "TOR_PHI", "CHI_NYR", "COL_NYI", "NJD_DAL", "CBJ_MIN", "VGK_NSH", "VAN_STL", "SEA_UTA", "SJS_WPG", "LAK_ANA", "EDM_CGY" });
                gameStringsNhl[32].AddRange(new string[] { "NJD_CAR", "NYI_CBJ", "NYR_VGK", "PHI_VAN", "PIT_SEA", "WSH_SJS", "BOS_LAK", "BUF_EDM", "CGY_DET", "ANA_FLA", "WPG_MTL", "UTA_OTT", "STL_TBL", "NSH_TOR", "MIN_CHI", "DAL_COL" });
                gameStringsNhl[33].AddRange(new string[] { "CAR_EDM", "LAK_CGY", "SJS_ANA", "SEA_WPG", "VAN_UTA", "VGK_STL", "CBJ_NSH", "NJD_MIN", "DAL_NYI", "COL_NYR", "CHI_PHI", "TOR_PIT", "TBL_WSH", "OTT_BOS", "MTL_BUF", "FLA_DET" });
                gameStringsNhl[34].AddRange(new string[] { "CAR_CBJ", "VGK_NJD", "VAN_NYI", "SEA_NYR", "SJS_PHI", "LAK_PIT", "EDM_WSH", "CGY_BOS", "BUF_ANA", "DET_WPG", "FLA_UTA", "MTL_STL", "OTT_NSH", "TBL_MIN", "TOR_DAL", "CHI_COL" });
                gameStringsNhl[35].AddRange(new string[] { "CAR_UTA", "WPG_STL", "ANA_NSH", "CGY_MIN", "EDM_DAL", "LAK_COL", "SJS_CHI", "SEA_TOR", "TBL_VAN", "OTT_VGK", "MTL_CBJ", "FLA_NJD", "DET_NYI", "BUF_NYR", "BOS_PHI", "WSH_PIT" });
                gameStringsNhl[36].AddRange(new string[] { "CAR_NSH", "STL_MIN", "UTA_DAL", "WPG_COL", "ANA_CHI", "CGY_TOR", "EDM_TBL", "LAK_OTT", "MTL_SJS", "FLA_SEA", "DET_VAN", "BUF_VGK", "BOS_CBJ", "WSH_NJD", "PIT_NYI", "PHI_NYR" });
                gameStringsNhl[37].AddRange(new string[] { "CAR_DET", "BUF_FLA", "BOS_MTL", "WSH_OTT", "PIT_TBL", "PHI_TOR", "NYR_CHI", "NYI_COL", "DAL_NJD", "MIN_CBJ", "NSH_VGK", "STL_VAN", "UTA_SEA", "WPG_SJS", "ANA_LAK", "CGY_EDM" });
                gameStringsNhl[38].AddRange(new string[] { "NYR_CAR", "PHI_NYI", "PIT_NJD", "WSH_CBJ", "BOS_VGK", "BUF_VAN", "DET_SEA", "FLA_SJS", "LAK_MTL", "EDM_OTT", "CGY_TBL", "ANA_TOR", "WPG_CHI", "UTA_COL", "STL_DAL", "NSH_MIN" });
                gameStringsNhl[39].AddRange(new string[] { "CAR_NJD", "CBJ_NYI", "VGK_NYR", "VAN_PHI", "SEA_PIT", "SJS_WSH", "LAK_BOS", "EDM_BUF", "DET_CGY", "FLA_ANA", "MTL_WPG", "OTT_UTA", "TBL_STL", "TOR_NSH", "CHI_MIN", "COL_DAL" });
                gameStringsNhl[40].AddRange(new string[] { "CAR_PIT", "PHI_WSH", "NYR_BOS", "NYI_BUF", "DET_NJD", "FLA_CBJ", "MTL_TOR", "OTT_TBL", "CHI_UTA", "STL_WPG", "NSH_ANA", "MIN_CGY", "EDM_DAL", "LAK_COL", "SJS_VGK", "SEA_VAN" });
                gameStringsNhl[41].AddRange(new string[] { "FLA_CAR", "MTL_DET", "OTT_BUF", "TBL_BOS", "TOR_WSH", "CHI_PIT", "COL_PHI", "DAL_NYR", "MIN_NYI", "NJD_NSH", "CBJ_STL", "VGK_UTA", "VAN_WPG", "SEA_ANA", "SJS_CGY", "LAK_EDM" });
                gameStringsNhl[42].AddRange(new string[] { "TBL_CAR", "TOR_OTT", "CHI_MTL", "COL_FLA", "DAL_DET", "MIN_BUF", "NSH_BOS", "STL_WSH", "PIT_UTA", "PHI_WPG", "NYR_ANA", "NYI_CGY", "NJD_EDM", "CBJ_LAK", "VGK_SJS", "VAN_SEA" });
                gameStringsNhl[43].AddRange(new string[] { "SJS_CAR", "LAK_SEA", "EDM_VAN", "CGY_VGK", "ANA_CBJ", "WPG_NJD", "UTA_NYI", "STL_NYR", "PHI_NSH", "PIT_MIN", "WSH_DAL", "BOS_COL", "BUF_CHI", "DET_TOR", "FLA_TBL", "MTL_OTT" });
                gameStringsNhl[44].AddRange(new string[] { "OTT_CAR", "TBL_MTL", "TOR_FLA", "CHI_DET", "COL_BUF", "DAL_BOS", "MIN_WSH", "NSH_PIT", "STL_PHI", "NYR_UTA", "NYI_WPG", "NJD_ANA", "CBJ_CGY", "VGK_EDM", "VAN_LAK", "SEA_SJS" });
                gameStringsNhl[45].AddRange(new string[] { "VAN_CAR", "SEA_VGK", "SJS_CBJ", "LAK_NJD", "EDM_NYI", "CGY_NYR", "ANA_PHI", "WPG_PIT", "WSH_UTA", "BOS_STL", "BUF_NSH", "DET_MIN", "FLA_DAL", "MTL_COL", "OTT_CHI", "TBL_TOR" });
                gameStringsNhl[46].AddRange(new string[] { "ANA_CAR", "WPG_CGY", "UTA_EDM", "STL_LAK", "NSH_SJS", "MIN_SEA", "DAL_VAN", "COL_VGK", "CBJ_CHI", "NJD_TOR", "NYI_TBL", "NYR_OTT", "PHI_MTL", "PIT_FLA", "WSH_DET", "BOS_BUF" });
                gameStringsNhl[47].AddRange(new string[] { "DET_CAR", "BUF_FLA", "BOS_MTL", "WSH_OTT", "TBL_PIT", "TOR_PHI", "CBJ_NYR", "NJD_NYI", "EDM_CHI", "CGY_LAK", "ANA_SJS", "WPG_SEA", "VAN_UTA", "VGK_STL", "COL_NSH", "DAL_MIN" });
                gameStringsNhl[48].AddRange(new string[] { "NYI_CAR", "NYR_NJD", "PHI_CBJ", "PIT_VGK", "WSH_VAN", "BOS_SEA", "BUF_SJS", "DET_LAK", "EDM_FLA", "CGY_MTL", "ANA_OTT", "WPG_TBL", "UTA_TOR", "STL_CHI", "NSH_COL", "MIN_DAL" });
                gameStringsNhl[49].AddRange(new string[] { "CAR_VAN", "VGK_SEA", "CBJ_SJS", "NJD_LAK", "NYI_EDM", "NYR_CGY", "PHI_ANA", "PIT_WPG", "UTA_WSH", "STL_BOS", "NSH_BUF", "MIN_DET", "DAL_FLA", "COL_MTL", "CHI_OTT", "TOR_TBL" });
                gameStringsNhl[50].AddRange(new string[] { "CAR_NJD", "CBJ_NYI", "TOR_NYR", "TBL_PHI", "PIT_OTT", "WSH_MTL", "BOS_FLA", "BUF_DET", "CHI_DAL", "COL_MIN", "VGK_NSH", "VAN_STL", "UTA_SEA", "WPG_SJS", "ANA_LAK", "CGY_EDM" });
                gameStringsNhl[51].AddRange(new string[] { "VGK_CAR", "VAN_CBJ", "SEA_NJD", "SJS_NYI", "LAK_NYR", "EDM_PHI", "CGY_PIT", "ANA_WSH", "BOS_WPG", "BUF_UTA", "DET_STL", "FLA_NSH", "MTL_MIN", "OTT_DAL", "TBL_COL", "TOR_CHI" });
                gameStringsNhl[52].AddRange(new string[] { "CAR_OTT", "MTL_TBL", "FLA_TOR", "DET_CHI", "BUF_COL", "BOS_DAL", "WSH_MIN", "PIT_NSH", "PHI_STL", "UTA_NYR", "WPG_NYI", "ANA_NJD", "CGY_CBJ", "EDM_VGK", "LAK_VAN", "SJS_SEA" });
                gameStringsNhl[53].AddRange(new string[] { "NSH_CAR", "MIN_STL", "DAL_UTA", "COL_WPG", "CHI_ANA", "TOR_CGY", "TBL_EDM", "OTT_LAK", "SJS_MTL", "SEA_FLA", "VAN_DET", "VGK_BUF", "CBJ_BOS", "NJD_WSH", "NYI_PIT", "NYR_PHI" });
                gameStringsNhl[54].AddRange(new string[] { "CAR_PHI", "NYR_PIT", "NYI_WSH", "NJD_BOS", "CBJ_BUF", "VGK_DET", "VAN_FLA", "SEA_MTL", "OTT_SJS", "TBL_LAK", "TOR_EDM", "CHI_CGY", "COL_ANA", "DAL_WPG", "MIN_UTA", "NSH_STL" });
                gameStringsNhl[55].AddRange(new string[] { "CAR_SJS", "SEA_LAK", "VAN_EDM", "VGK_CGY", "CBJ_ANA", "NJD_WPG", "NYI_UTA", "NYR_STL", "NSH_PHI", "MIN_PIT", "DAL_WSH", "COL_BOS", "CHI_BUF", "TOR_DET", "TBL_FLA", "OTT_MTL" });
                gameStringsNhl[56].AddRange(new string[] { "MIN_CAR", "DAL_NSH", "COL_STL", "CHI_UTA", "TOR_WPG", "TBL_ANA", "OTT_CGY", "MTL_EDM", "FLA_LAK", "SJS_DET", "SEA_BUF", "VAN_BOS", "VGK_WSH", "CBJ_PIT", "NJD_PHI", "NYI_NYR" });
                gameStringsNhl[57].AddRange(new string[] { "CAR_NYI", "NJD_NYR", "CBJ_PHI", "TOR_PIT", "WSH_TBL", "BOS_OTT", "BUF_MTL", "DET_FLA", "CHI_MIN", "DAL_NSH", "COL_STL", "VGK_UTA", "WPG_VAN", "ANA_SEA", "CGY_SJS", "EDM_LAK" });
                gameStringsNhl[58].AddRange(new string[] { "CAR_PHI", "NYR_PIT", "NYI_WSH", "NJD_BOS", "CBJ_BUF", "DET_TOR", "FLA_TBL", "MTL_OTT", "CHI_STL", "NSH_UTA", "MIN_WPG", "DAL_ANA", "COL_CGY", "EDM_VGK", "LAK_VAN", "SJS_SEA" });
                gameStringsNhl[59].AddRange(new string[] { "WSH_CAR", "BOS_PIT", "BUF_PHI", "DET_NYR", "FLA_NYI", "MTL_NJD", "OTT_CBJ", "TBL_VGK", "VAN_TOR", "SEA_CHI", "SJS_COL", "LAK_DAL", "EDM_MIN", "CGY_NSH", "ANA_STL", "WPG_UTA" });
                gameStringsNhl[60].AddRange(new string[] { "CAR_TBL", "OTT_TOR", "MTL_CHI", "FLA_COL", "DET_DAL", "BUF_MIN", "BOS_NSH", "WSH_STL", "UTA_PIT", "WPG_PHI", "ANA_NYR", "CGY_NYI", "EDM_NJD", "LAK_CBJ", "SJS_VGK", "SEA_VAN" });
                gameStringsNhl[61].AddRange(new string[] { "COL_CAR", "CHI_DAL", "TOR_MIN", "TBL_NSH", "OTT_STL", "MTL_UTA", "FLA_WPG", "DET_ANA", "BUF_CGY", "EDM_BOS", "LAK_WSH", "SJS_PIT", "SEA_PHI", "VAN_NYR", "VGK_NYI", "CBJ_NJD" });
                gameStringsNhl[62].AddRange(new string[] { "STL_CAR", "NSH_UTA", "MIN_WPG", "DAL_ANA", "COL_CGY", "CHI_EDM", "TOR_LAK", "TBL_SJS", "OTT_SEA", "VAN_MTL", "VGK_FLA", "CBJ_DET", "NJD_BUF", "NYI_BOS", "NYR_WSH", "PHI_PIT" });
                gameStringsNhl[63].AddRange(new string[] { "WPG_CAR", "UTA_ANA", "STL_CGY", "NSH_EDM", "MIN_LAK", "DAL_SJS", "COL_SEA", "CHI_VAN", "TOR_VGK", "CBJ_TBL", "NJD_OTT", "NYI_MTL", "NYR_FLA", "PHI_DET", "PIT_BUF", "WSH_BOS" });
                gameStringsNhl[64].AddRange(new string[] { "TBL_CAR", "OTT_TOR", "MTL_CBJ", "FLA_NJD", "NYI_DET", "NYR_BUF", "PHI_BOS", "PIT_WSH", "VAN_CHI", "SEA_VGK", "SJS_COL", "LAK_DAL", "MIN_EDM", "NSH_CGY", "STL_ANA", "UTA_WPG" });
                gameStringsNhl[65].AddRange(new string[] { "CAR_MIN", "NSH_DAL", "STL_COL", "UTA_CHI", "WPG_TOR", "ANA_TBL", "CGY_OTT", "EDM_MTL", "LAK_FLA", "DET_SJS", "BUF_SEA", "BOS_VAN", "WSH_VGK", "PIT_CBJ", "PHI_NJD", "NYR_NYI" });
                gameStringsNhl[66].AddRange(new string[] { "MTL_CAR", "FLA_OTT", "DET_TBL", "BUF_TOR", "CBJ_BOS", "NJD_WSH", "NYI_PIT", "NYR_PHI", "SJS_CHI", "LAK_SEA", "EDM_VAN", "CGY_VGK", "COL_ANA", "DAL_WPG", "MIN_UTA", "NSH_STL" });
                gameStringsNhl[67].AddRange(new string[] { "BOS_CAR", "WSH_BUF", "PIT_DET", "PHI_FLA", "MTL_NYR", "OTT_NYI", "TBL_NJD", "TOR_CBJ", "ANA_CHI", "WPG_CGY", "UTA_EDM", "STL_LAK", "SJS_NSH", "SEA_MIN", "VAN_DAL", "VGK_COL" });
                gameStringsNhl[68].AddRange(new string[] { "CAR_WSH", "PIT_BOS", "PHI_BUF", "NYR_DET", "NYI_FLA", "MTL_NJD", "OTT_CBJ", "TBL_TOR", "CHI_WPG", "UTA_ANA", "STL_CGY", "NSH_EDM", "MIN_LAK", "SJS_DAL", "SEA_COL", "VAN_VGK" });
                gameStringsNhl[69].AddRange(new string[] { "CAR_FLA", "DET_MTL", "BUF_OTT", "BOS_TBL", "WSH_TOR", "PIT_CHI", "PHI_COL", "NYR_DAL", "NYI_MIN", "NSH_NJD", "STL_CBJ", "UTA_VGK", "WPG_VAN", "ANA_SEA", "CGY_SJS", "EDM_LAK" });
                gameStringsNhl[70].AddRange(new string[] { "CAR_NYR", "NYI_PHI", "NJD_PIT", "CBJ_WSH", "BOS_TOR", "BUF_TBL", "DET_OTT", "FLA_MTL", "CHI_NSH", "MIN_STL", "DAL_UTA", "COL_WPG", "ANA_VGK", "CGY_VAN", "EDM_SEA", "LAK_SJS" });
                gameStringsNhl[71].AddRange(new string[] { "DAL_CAR", "COL_MIN", "CHI_NSH", "TOR_STL", "TBL_UTA", "OTT_WPG", "MTL_ANA", "FLA_CGY", "EDM_DET", "LAK_BUF", "SJS_BOS", "SEA_WSH", "VAN_PIT", "VGK_PHI", "CBJ_NYR", "NJD_NYI" });
                gameStringsNhl[72].AddRange(new string[] { "TOR_CAR", "CHI_TBL", "COL_OTT", "DAL_MTL", "MIN_FLA", "NSH_DET", "STL_BUF", "UTA_BOS", "WPG_WSH", "PIT_ANA", "PHI_CGY", "NYR_EDM", "NYI_LAK", "NJD_SJS", "CBJ_SEA", "VGK_VAN" });
                gameStringsNhl[73].AddRange(new string[] { "FLA_CAR", "DET_MTL", "BUF_OTT", "BOS_TBL", "WSH_TOR", "CBJ_PIT", "NJD_PHI", "NYI_NYR", "LAK_CHI", "EDM_SJS", "CGY_SEA", "ANA_VAN", "WPG_VGK", "COL_UTA", "DAL_STL", "MIN_NSH" });
                gameStringsNhl[74].AddRange(new string[] { "MTL_CAR", "OTT_FLA", "TBL_DET", "TOR_BUF", "CHI_BOS", "COL_WSH", "DAL_PIT", "MIN_PHI", "NYR_NSH", "NYI_STL", "NJD_UTA", "CBJ_WPG", "VGK_ANA", "VAN_CGY", "SEA_EDM", "SJS_LAK" });
                gameStringsNhl[75].AddRange(new string[] { "CAR_CGY", "EDM_ANA", "LAK_WPG", "SJS_UTA", "SEA_STL", "VAN_NSH", "VGK_MIN", "CBJ_DAL", "COL_NJD", "CHI_NYI", "TOR_NYR", "TBL_PHI", "OTT_PIT", "MTL_WSH", "FLA_BOS", "DET_BUF" });
                gameStringsNhl[76].AddRange(new string[] { "BUF_CAR", "BOS_DET", "WSH_FLA", "PIT_MTL", "PHI_OTT", "TBL_NYR", "TOR_NYI", "CBJ_NJD", "CGY_CHI", "ANA_EDM", "WPG_LAK", "UTA_SJS", "STL_SEA", "VAN_NSH", "VGK_MIN", "COL_DAL" });
                gameStringsNhl[77].AddRange(new string[] { "UTA_CAR", "STL_WPG", "NSH_ANA", "MIN_CGY", "DAL_EDM", "COL_LAK", "CHI_SJS", "TOR_SEA", "VAN_TBL", "VGK_OTT", "CBJ_MTL", "NJD_FLA", "NYI_DET", "NYR_BUF", "PHI_BOS", "PIT_WSH" });
                gameStringsNhl[78].AddRange(new string[] { "CAR_STL", "UTA_NSH", "WPG_MIN", "ANA_DAL", "CGY_COL", "EDM_CHI", "LAK_TOR", "SJS_TBL", "SEA_OTT", "MTL_VAN", "FLA_VGK", "DET_CBJ", "BUF_NJD", "BOS_NYI", "WSH_NYR", "PIT_PHI" });
                gameStringsNhl[79].AddRange(new string[] { "CHI_CAR", "TOR_COL", "TBL_DAL", "OTT_MIN", "MTL_NSH", "FLA_STL", "DET_UTA", "BUF_WPG", "ANA_BOS", "CGY_WSH", "EDM_PIT", "LAK_PHI", "SJS_NYR", "SEA_NYI", "VAN_NJD", "VGK_CBJ" });
                gameStringsNhl[80].AddRange(new string[] { "SEA_CAR", "SJS_VAN", "LAK_VGK", "EDM_CBJ", "CGY_NJD", "ANA_NYI", "WPG_NYR", "UTA_PHI", "PIT_STL", "WSH_NSH", "BOS_MIN", "BUF_DAL", "DET_COL", "FLA_CHI", "MTL_TOR", "OTT_TBL" });
                gameStringsNhl[81].AddRange(new string[] { "PHI_CAR", "PIT_NYR", "WSH_NYI", "BOS_NJD", "BUF_CBJ", "DET_VGK", "FLA_VAN", "MTL_SEA", "SJS_OTT", "LAK_TBL", "EDM_TOR", "CGY_CHI", "ANA_COL", "WPG_DAL", "UTA_MIN", "STL_NSH" });
                gameStringsNhl[82].AddRange(new string[] { "BOS_CAR", "BUF_WSH", "DET_PIT", "FLA_PHI", "MTL_NYR", "OTT_NYI", "TBL_NJD", "TOR_CBJ", "VGK_CHI", "VAN_COL", "SEA_DAL", "SJS_MIN", "LAK_NSH", "EDM_STL", "CGY_UTA", "ANA_WPG" });
                gameStringsPwhl[1].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[2].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[3].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[4].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[5].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[6].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[7].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[8].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[9].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[10].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[11].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[12].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[13].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[14].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[15].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[16].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[17].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[18].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[19].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[20].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[21].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[22].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[23].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[24].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[25].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[26].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[27].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[28].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[29].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[30].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[31].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[32].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[33].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[34].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[35].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[36].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[37].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[38].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[39].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[40].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[41].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[42].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[43].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[44].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[45].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[46].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[47].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[48].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[49].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[50].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[51].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[52].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[53].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[54].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[55].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[56].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[57].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[58].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[59].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[60].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[61].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[62].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[63].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[64].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[65].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[66].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[67].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[68].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[69].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[70].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                gameStringsPwhl[71].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[72].AddRange(new string[] { "BOS_NYC", "MIN_MTL", "OTT_VAN", "SEA_TOR" });
                gameStringsPwhl[73].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[74].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[75].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[76].AddRange(new string[] { "BOS_OTT", "MIN_SEA", "MTL_TOR", "NYC_VAN" });
                gameStringsPwhl[77].AddRange(new string[] { "BOS_TOR", "MIN_VAN", "MTL_OTT", "NYC_SEA" });
                gameStringsPwhl[78].AddRange(new string[] { "BOS_VAN", "MIN_TOR", "MTL_SEA", "NYC_OTT" });
                gameStringsPwhl[79].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[80].AddRange(new string[] { "BOS_MIN", "MTL_NYC", "OTT_SEA", "TOR_VAN" });
                gameStringsPwhl[81].AddRange(new string[] { "BOS_MTL", "MIN_NYC", "OTT_TOR", "SEA_VAN" });
                gameStringsPwhl[82].AddRange(new string[] { "BOS_SEA", "MIN_OTT", "MTL_VAN", "NYC_TOR" });
                break;
        }

        List<string> nightGamesNhl = gameStringsNhl[nightIndex];
        List<string> nightGamesPwhl = gameStringsPwhl[nightIndex];

        if (userLeague == "NHL")
        {
            if (nightGamesNhl.Count < gameIndex)
            {
                return string.Empty;
            }

            else
            {
                return nightGamesNhl[gameIndex];
            }
        }

        else if (userLeague == "PWHL")
        {
            if (nightGamesPwhl.Count < gameIndex)
            {
                return string.Empty;
            }

            else
            {
                return nightGamesPwhl[gameIndex];
            }
        }

        else
        {
            return string.Empty;
        }
    }
#endregion
}}
