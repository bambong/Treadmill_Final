using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Gymchair.Core.Mgr
{
    public class DataMgr : Behaviour.ManagerBehaviour<DataMgr>
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
        public UserData.UserData UserData { get => _userData; }

        public void AddGymData(UserGymData gymData)
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

        public UserGymData GetGymData(int key)
        {
            string gymText = PlayerPrefs.GetString($"{UserName}_Gymchair_{key}");
            return JsonConvert.DeserializeObject<UserGymData>(gymText);
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

        public override void OnCoreMessage(object msg)
        {
        }
    }
}