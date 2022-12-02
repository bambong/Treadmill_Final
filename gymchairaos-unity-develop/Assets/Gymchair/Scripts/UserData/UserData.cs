using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gymchair.UserData
{
    [Serializable]
    public class UserData
    {
        public string nick;
        public string name;
        public int sex; // 0 : 남자, 1 : 여자

        public int birth_year;
        public int birth_month;
        public int birth_day;

        public float height; // 키
        public float weight; // 몸무게
        public float bmi; // BMI

        public string gym_info; // 운동 정보
        public string disabled_info; // 장애 정보
        public string[] disease_list; // 질병 정보

        public string note; // 비장애인 하는 이유

        public int tutorial; // 튜토리얼 여부
        public int play_count; // 플레이 횟수  
    }
}