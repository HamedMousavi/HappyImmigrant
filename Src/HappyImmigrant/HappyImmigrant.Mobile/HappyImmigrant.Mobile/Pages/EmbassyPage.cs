using System.Collections.Generic;
using System.IO;
using System.Linq;
using HappyImmigrant.Mobile.Data;
using Xamarin.Forms;

namespace HappyImmigrant.Mobile.Pages
{
    public class EmbassyPage : ContentPage
    {
        public EmbassyPage()
        {
            var layout = new StackLayout {VerticalOptions = LayoutOptions.Center};

            var countries = new CountriesDb().GetAll().ToList();
            foreach (var country in countries)
            {
                var item = new StackLayout { VerticalOptions = LayoutOptions.Center, Orientation = StackOrientation.Horizontal };
                if (country.FlagImage != null)
                {
                    item.Children.Add(new Image
                    {
                        Source = ImageSource.FromStream(() => new MemoryStream(country.FlagImage))
                    });
                }
                item.Children.Add(new Label{Text = country.CountryName});
                layout.Children.Add(item);
                //label.Text += string.Format("{0}\n\n", country.CountryName);
            }

            Content = new ScrollView { Content = layout };
        }
    }
}
