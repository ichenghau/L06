﻿using System;
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
        private Button searchButton;
        private Entry cityEntry;
        private Entry areaEntry;
        private List<FamilyStore> myStoreDataList;
        private readonly WebApiServices myWebApiService;
        public MyListViewPage(string title)
        {
            myStoreDataList = new List<FamilyStore>();
            myWebApiService = new WebApiServices();

            searchButton = new Button {Text = "Search"};
            cityEntry = new Entry { Placeholder = "請輸入城市名稱" };
            areaEntry = new Entry{ Placeholder = "請輸入行政區域"};

            searchButton.Clicked += async (sender, e) =>
            {
                if (cityEntry != null && cityEntry.Text == string.Empty)
                {
                    cityEntry.Text = "台北市";
                }
                if (areaEntry != null && areaEntry.Text == string.Empty)
                {
                    areaEntry.Text = "大安區";
                }
                var resultData = await myWebApiService.GetDataAsync(cityEntry.Text, areaEntry.Text);
                myStoreDataList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FamilyStore>>(resultData);

                Debug.WriteLine(myStoreDataList.Count);
            };

            Title = title;
            var listView = new ListView
            {
                IsPullToRefreshEnabled = true,
                RowHeight = 80,
                ItemsSource = new[]
                {
                    new StoreData {Name = "全家大安店", Address = "台北市大安區大安路一段20號", Tel = "02-27117896"},
                    new StoreData {Name = "全家仁慈店", Address = "台北市大安區仁愛路四段48巷6號", Tel = "02-27089002"},
                    new StoreData {Name = "全家明曜店", Address = "台北市大安區仁愛路四段151巷34號", Tel = "02-27780326"},
                    new StoreData {Name = "全家國泰店", Address = "台北市大安區仁愛路四段266巷15弄10號", Tel = "02-27542056"},
                    new StoreData {Name = "全家忠愛店", Address = "台北市大安區仁愛路四段27巷43號", Tel = "02-27314580"},
                },
                ItemTemplate = new DataTemplate(typeof (MyListViewCell))
            };


            listView.ItemTapped += (sender, e) =>
            {
                var baseUrl = "https://www.google.com.tw/maps/place/";
                var storeData = e.Item as StoreData;
               
                if (storeData != null)
                    Device.OpenUri(new Uri( $"{baseUrl}{storeData.Address}"));

                ((ListView)sender).SelectedItem = null;
            };

            Padding = new Thickness(0, 20, 0, 0);
            Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    cityEntry,
                    areaEntry,
                    searchButton,
                    new Label
                    {
                        HorizontalTextAlignment= TextAlignment.Center,
                        Text = Title,
                        FontSize = 30
                    },
                    listView
                }
            };
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
