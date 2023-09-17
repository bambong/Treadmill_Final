using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Gymchair.Core.Mgr
{
    public class DataManager 
    { 
        // 로그인한 유저 이름
        private string _userName;
        public string UserName
        {
            set
            {
                _userName = value;

                string text = PlayerPrefs.GetString(value);
                _userData = JsonUtility.FromJson<UserData.UserData>(text);
            }
            get
            {
                return _userName;
            }
        }

        private UserData.UserData _userData;
        public UserData.UserData UserData
                {
                    get
                    {
                        if (_userData == null) 
                        {
                             string text = PlayerPrefs.GetString(_userName);
                            _userData = JsonUtility.FromJson<UserData.UserData>(text);
                        }
                        return _userData;
                    }
                }


        int _key_number = 0;
        
        public void AddGymData(UserGymData gymData, GymchairData[] gymchairDatas)
        {
            int keyNumber = 0;

            List<string> gymDatas = new List<string>();

            if (PlayerPrefs.HasKey($"{UserName}_Gymchair"))
            {
                string text = PlayerPrefs.GetString($"{UserName}_Gymchair");
                string[] datas = JsonConvert.DeserializeObject<string[]>(text);
                gymDatas = new List<string>(datas);
            }
            keyNumber = gymDatas.Count;

            gymDatas.Add(gymData.gymDate);

            PlayerPrefs.SetString($"{UserName}_Gymchair", JsonConvert.SerializeObject(gymDatas.ToArray()));
            PlayerPrefs.SetString($"{UserName}_Gymchair_{keyNumber}", JsonConvert.SerializeObject(gymData));
            PlayerPrefs.SetString($"{UserName}_Gymchair_{keyNumber}_Datas", JsonConvert.SerializeObject(gymchairDatas));
            PlayerPrefs.Save();
        }

        public UserGymData GetLastGymData()
        {
            UserGymData gymData = null;

            List<string> gymDatas = new List<string>();

            if (PlayerPrefs.HasKey($"{UserName}_Gymchair"))
            {
                string text = PlayerPrefs.GetString($"{UserName}_Gymchair");
                string[] datas = JsonConvert.DeserializeObject<string[]>(text);
                gymDatas = new List<string>(datas);

                int keyNumber = gymDatas.Count - 1;
                string gymText = PlayerPrefs.GetString($"{UserName}_Gymchair_{keyNumber}");
                gymData = JsonConvert.DeserializeObject<UserGymData>(gymText);
            }

            return gymData;
        }

        public GymchairData[] GetLastGymTickData()
        {
            GymchairData[] gymchairDatas = null;

            List<string> gymDatas = new List<string>();

            if (PlayerPrefs.HasKey($"{UserName}_Gymchair"))
            {
                string text = PlayerPrefs.GetString($"{UserName}_Gymchair");
                string[] datas = JsonConvert.DeserializeObject<string[]>(text);
                gymDatas = new List<string>(datas);

                int keyNumber = gymDatas.Count - 1;
                string gymText = PlayerPrefs.GetString($"{UserName}_Gymchair_{keyNumber}_Datas");
                gymchairDatas = JsonConvert.DeserializeObject<GymchairData[]>(gymText);
            }

            return gymchairDatas;
        }

        public int GetKeyNumber()
        {
            return _key_number;
        }

        public void SetKeyNumber(int keyNumber)
        {
            _key_number = keyNumber;
        }

        public UserGymData GetTargetGymData(int keynumber)
        {
            UserGymData gymData = null;

            List<string> gymDatas = new List<string>();

            if (PlayerPrefs.HasKey($"{UserName}_Gymchair"))
            {
                string gymText = PlayerPrefs.GetString($"{UserName}_Gymchair_{keynumber}");
                gymData = JsonConvert.DeserializeObject<UserGymData>(gymText);
            }

            return gymData;
        }

        public GymchairData[] GetTargetGymTickData(int keynumber)
        {
            GymchairData[] gymchairDatas = null;

            List<string> gymDatas = new List<string>();

            if (PlayerPrefs.HasKey($"{UserName}_Gymchair"))
            {
                string gymText = PlayerPrefs.GetString($"{UserName}_Gymchair_{keynumber}_Datas");
                gymchairDatas = JsonConvert.DeserializeObject<GymchairData[]>(gymText);
            }

            return gymchairDatas;
        }

        public UserGymData GetGymData(int key)
        {
            string gymText = PlayerPrefs.GetString($"{UserName}_Gymchair_{key}");
            return JsonConvert.DeserializeObject<UserGymData>(gymText);
        }

        public GymchairData[] GetGymTickData(int key)
        {
            string gymText = PlayerPrefs.GetString($"{UserName}_Gymchair_{key}_Datas");
            return JsonConvert.DeserializeObject<GymchairData[]>(gymText);
        }

        public string[] GetGymList()
        {
            string[] gyms = null;

            if (PlayerPrefs.HasKey($"{UserName}_Gymchair"))
            {
                string text = PlayerPrefs.GetString($"{UserName}_Gymchair");
                gyms = JsonConvert.DeserializeObject<string[]>(text);
            }

            return gyms;
        }

    }
}