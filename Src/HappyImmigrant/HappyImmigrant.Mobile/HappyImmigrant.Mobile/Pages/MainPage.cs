using Xamarin.Forms;

namespace HappyImmigrant.Mobile.Pages
{
    public class MainPage : TabbedPage
    {
        public MainPage()
        {
            Children.Add(new HomePage { Title = "اخبار" });
            Children.Add(new EmbassyPage { Title = "سفارت" });
            //Children.Add(new LedgerPage() {Title = "Ledger", Icon = "Ledger.png"});
            //Children.Add(new TaskPage() {Title = "Tasks", Icon = "Tasks.png"});
            //Children.Add(new AlertPage() {Title = "Alerts", Icon = "Alerts.png"});
        }
    }
}