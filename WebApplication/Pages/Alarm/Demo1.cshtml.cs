using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace WebApplication.Pages.Alarm
{
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


    public class Demo1Model : PageModel
    {
        // 98, 108

        public List<AlarmModel> mAlarmModelList = new List<AlarmModel>();


        public Demo1Model()
        {
            mAlarmModelList.Add(new AlarmModel("Fabryka_EVO", "Alarm nr 011"));
            mAlarmModelList.Add(new AlarmModel("Fabryka_EVO", "Alarm nr 083"));
            mAlarmModelList.Add(new AlarmModel("Fabryka_EVO", "Alarm nr 098"));
            mAlarmModelList.Add(new AlarmModel("Fabryka_EVO", "Alarm nr 109"));
            mAlarmModelList.Add(new AlarmModel("Fabryka_EVO", "Alarm nr 155"));
            mAlarmModelList.Add(new AlarmModel("Fabryka_EVO", "Alarm nr 202"));
            mAlarmModelList.Add(new AlarmModel("Fabryka_EVO", "Alarm nr 227"));
            mAlarmModelList.Add(new AlarmModel("Fabryka_EVO", "Alarm nr 'test'"));
        }

        public void OnGet()
        {
            ReadAlarmValue();
        }


        void ReadAlarmValue()
        {
            AsixRestClient asixRestClient = new AsixRestClient();

            foreach (var alarmModel in mAlarmModelList)
                alarmModel.mAlarmState = asixRestClient.ReadAlarmState(alarmModel.mDomain, alarmModel.mName);
        }
    }

}