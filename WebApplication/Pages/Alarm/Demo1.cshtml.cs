using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace WebApplication.Pages.Alarm
{
    /// <summary>
    /// Reprezentuje przeczytany stan alarmu
    /// </summary>
    public class AlarmModel
    {
        public AlarmModel(string aDomain, string aName)
        {
            mDomain = aDomain;
            mName = aName;
        }

        public string mDomain, mName;
        public AlarmState mAlarmState;
    }


    /// <summary>
    /// Przykład odczytu i wyświetlenia stanu alarmów bieżąych
    /// </summary>
    public class Demo1Model : PageModel
    {
        /// <summary>
        /// Przechowuje przeczytane stany alarmów
        /// </summary>
        public List<AlarmModel> mAlarmModelList = new List<AlarmModel>();


        /// <summary>
        /// Konstruktor klasy modelu strony 
        /// </summary>
        public Demo1Model()
        {
            mAlarmModelList.Add(new AlarmModel("Fabryka_EVO", "Alarm nr 011"));
            mAlarmModelList.Add(new AlarmModel("Fabryka_EVO", "Alarm nr 083"));
            mAlarmModelList.Add(new AlarmModel("Fabryka_EVO", "Alarm nr 098"));
            mAlarmModelList.Add(new AlarmModel("Fabryka_EVO", "Alarm nr 109"));
            mAlarmModelList.Add(new AlarmModel("Fabryka_EVO", "Alarm nr 155"));
            mAlarmModelList.Add(new AlarmModel("Fabryka_EVO", "Alarm nr 202"));
            mAlarmModelList.Add(new AlarmModel("Fabryka_EVO", "Alarm nr 227"));
            mAlarmModelList.Add(new AlarmModel("Fabryka_EVO", "Alarm_AAA"));     // nieistniejąca nazwa alarmu, demonstruje obsługę błędu odczytu alarmu
        }


        /// <summary>
        /// Funkcja wywoływana przy pobieraniu strony przez przeglądarkę
        /// </summary>
        public void OnGet()
        {
            ReadAlarmValue();
        }


        /// <summary>
        /// Funkcja realizująca odczyt stanu alarmów
        /// </summary>
        void ReadAlarmValue()
        {
            AsixRestClient asixRestClient = new AsixRestClient();

            foreach (var alarmModel in mAlarmModelList)
                alarmModel.mAlarmState = asixRestClient.ReadAlarmState(alarmModel.mDomain, alarmModel.mName);
        }
    }
}