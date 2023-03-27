using Microsoft.AspNetCore.Mvc.RazorPages;
using Asix;


namespace WebApplication.Pages.Alarm
{
    /// <summary>
    /// Reprezentuje przeczytany stan alarmu
    /// </summary>
    public class AlarmModel
    {
        public string Name;
        public AlarmState AlarmState = new AlarmState();

        public AlarmModel(string aName)
        {
            Name = aName;
        }
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
            mAlarmModelList.Add(new AlarmModel("Alarm nr 105"));
            mAlarmModelList.Add(new AlarmModel("Alarm nr 106"));
            mAlarmModelList.Add(new AlarmModel("Alarm nr 107"));
            mAlarmModelList.Add(new AlarmModel("Alarm nr 109"));
            mAlarmModelList.Add(new AlarmModel("Alarm nr 155"));
            mAlarmModelList.Add(new AlarmModel("Alarm nr 202"));
            mAlarmModelList.Add(new AlarmModel("Alarm nr 213"));
            mAlarmModelList.Add(new AlarmModel("Alarm_AAA"));     // nieistniejąca nazwa alarmu, demonstruje obsługę błędu odczytu alarmu
        }


        /// <summary>
        /// Funkcja wywoływana przy pobieraniu strony przez przeglądarkę
        /// </summary>
        public async Task OnGet()
        {
            await ReadAlarmValue();
        }


        /// <summary>
        /// Funkcja realizująca odczyt stanu alarmów
        /// </summary>
        async Task ReadAlarmValue()
        {
            AsixRestClient asixRestClient = AsixRestClient.Create();

            IEnumerable<string> alarmNames = mAlarmModelList.Select(x => x.Name);
            ICollection<AlarmState> alarmStates = await asixRestClient.GetAlarmsStateAsync(AsixRestClient.AlarmDomainName, alarmNames);
            AlarmState[] alarmStatesArray = alarmStates.ToArray();

            for (int i = 0; i < mAlarmModelList.Count; i++)
            {
                AlarmModel alarmModel = mAlarmModelList[i];
                AlarmState alarmState = alarmStatesArray[i];
                alarmModel.AlarmState = alarmState;
            }
        }
    }
}