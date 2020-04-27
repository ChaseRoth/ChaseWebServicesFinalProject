using ChaseWebServicesFinalProject.Pages.NavItems;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace ChaseWebServicesFinalProject.Pages
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainMasterPage : MasterDetailPage
    {
        public MainMasterPage()
        {
            InitializeComponent();
            masterPage.MasterPageNavListView.ItemTapped += OnMasterPageListView_ItemTapped;
            //masterPage.MasterPageActions.ItemTapped += OnMasterPageListView_ItemTapped;            
        }

        /// <summary>
        ///     Handler for both the main listview for navigating and the listview for actions
        /// </summary>
        void OnMasterPageListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            #pragma warning disable IDE0019 // Use pattern matching
            var item = e.Item as MasterPageItem;
            #pragma warning restore IDE0019 // Use pattern matching
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                masterPage.MasterPageNavListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}
