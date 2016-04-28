using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using L05_2.MyServices;
using Xamarin.Forms;

namespace L05_2
{
    public class MyListViewPage : ContentPage
    {
        private Picker cityEntry;
        private Picker areaEntry;
        private List<FamilyStore> myStoreDataList;
        private ListView listView;
        private StackLayout stackLayout;
        private readonly WebApiServices myWebApiService;
        public MyListViewPage(string title)
        {
            myStoreDataList = new List<FamilyStore>();
            myWebApiService = new WebApiServices();
            cityEntry = new Picker() { Title = "城市" };
            areaEntry = new Picker() { Title="區域" };
            
            foreach (var item in GetTown())
            {
                cityEntry.Items.Add(item);
            }
            cityEntry.SelectedIndexChanged += CityEntry_SelectedIndexChanged;


            foreach (var item in GetStoreTown().Where(w=> cityEntry.Items[0]==w.City))
            {
                areaEntry.Items.Add(item.Town);
            }

            areaEntry.SelectedIndexChanged += AreaEntry_SelectedIndexChanged;

            Title = title;

            stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    cityEntry,
                    areaEntry,
                    //searchButton,
                    new Label
                    {
                        HorizontalTextAlignment= TextAlignment.Center,
                        Text = Title,
                        FontSize = 30
                    }
                }
            };
            Padding = new Thickness(0, 20, 0, 0);
            Content = stackLayout;
        }

        private void CityEntry_SelectedIndexChanged(object sender, EventArgs e)
        {
            areaEntry.Items.Clear();
            foreach (var item in GetStoreTown().Where(w => cityEntry.Items[cityEntry.SelectedIndex] == w.City))
            {
                areaEntry.Items.Add(item.Town);
            }
        }

        private async void AreaEntry_SelectedIndexChanged(object sender, EventArgs e)
        {
            var resultData = await myWebApiService.GetDataAsync(cityEntry.Items[cityEntry.SelectedIndex], areaEntry.Items[areaEntry.SelectedIndex]);
            myStoreDataList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FamilyStore>>(resultData);

            listView = new ListView
            {
                IsPullToRefreshEnabled = true,
                RowHeight = 80,
                ItemsSource = myStoreDataList,
                ItemTemplate = new DataTemplate(typeof(MyListViewCell))
            };

            listView.ItemTapped += ListView_ItemTapped;

            stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    cityEntry,
                    areaEntry,
                    new Label
                    {
                        HorizontalTextAlignment= TextAlignment.Center,
                        Text = Title,
                        FontSize = 30
                    },
                    listView
                }
            };

            Content = stackLayout;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var baseUrl = "https://www.google.com.tw/maps/place/";
            var storeData = e.Item as FamilyStore;

            if (storeData != null)
                Device.OpenUri(new Uri($"{baseUrl}{storeData.addr}"));

            ((ListView)sender).SelectedItem = null;
        }

        private List<string> GetTown()
        {
            var rtn = new List<string>();

            rtn.Add("台北市");
            rtn.Add("新北市");

            return rtn;

        }

        private List<StoreTown> GetStoreTown()
        {
            var rtn = new List<StoreTown>();

            rtn.Add(new StoreTown() {
                 City= "台北市",
                 Town= "中正區"
            });
            rtn.Add(new StoreTown()
            {
                City = "台北市",
                Town = "大同區"
            });
            rtn.Add(new StoreTown()
            {
                City = "台北市",
                Town = "中山區"
            });
            rtn.Add(new StoreTown()
            {
                City = "台北市",
                Town = "松山區"
            });
            rtn.Add(new StoreTown()
            {
                City = "台北市",
                Town = "大安區"
            });
            rtn.Add(new StoreTown()
            {
                City = "新北市",
                Town = "萬里區"
            });
            rtn.Add(new StoreTown()
            {
                City = "新北市",
                Town = "金山區"
            });
            rtn.Add(new StoreTown()
            {
                City = "新北市",
                Town = "板橋區"
            });
            rtn.Add(new StoreTown()
            {
                City = "新北市",
                Town = "汐止區"
            });
            rtn.Add(new StoreTown()
            {
                City = "新北市",
                Town = "深坑區"
            });

            return rtn;
        }

        private class StoreTown
        {
            /// <summary>
            /// 區
            /// </summary>
            public string Town { get; set; }

            /// <summary>
            /// 城市
            /// </summary>
            public string City { get; set; }

        }
    }

    public class StoreData
    {
        public string Name { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
    }

    public class FamilyStore
    {
        public string NAME { get; set; }
        public string TEL { get; set; }
        public string POSTel { get; set; }
        public double px { get; set; }
        public double py { get; set; }
        public string addr { get; set; }
        public double SERID { get; set; }
        public string pkey { get; set; }
        public string oldpkey { get; set; }
        public string post { get; set; }
        public string all { get; set; }
        public string road { get; set; }
        public object twoice { get; set; }
    }

   


}
