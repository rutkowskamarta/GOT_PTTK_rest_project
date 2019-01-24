﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAndroidMockup.ViewModels;
using WpfAndroidMockup.Models;

namespace WpfAndroidMockup.Views
{
    /// <summary>
    /// Logika widoku PotwierdzanieOdbytejWycieczkiPrzodownikView.xaml
    /// </summary>
    public partial class PotwierdzanieOdbytejWycieczkiPrzodownikView : UserControl
    {
        /// <summary>
        /// View model wycieczki
        /// </summary>
        public WycieczkaViewModel wycieczkaViewModel;
        private Grid previousGridToClose;

        /// <summary>
        /// Konstruktor nieparametryczny widoku potwierdzania wycieczki przez przodownika
        /// </summary>
        public PotwierdzanieOdbytejWycieczkiPrzodownikView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Metoda wyświetlająca komunikat o pustej liście wycieczek do potwierdzenia
        /// </summary>
        public void ZareagujGdyListaPusta()
        {
            if (wycieczkaViewModel.WycieczkiObservableCollection.Count == 0)
            {
                WyswietlKomunikat("BRAK WYCIECZEK DO POTWIERDZENIA");
            }
        }

        /// <summary>
        /// Logika przycisku na element z listy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewItem_OnPressed(object sender, MouseButtonEventArgs e)
        {
            ListBox listView = sender as ListBox;
            if (listView.SelectedItem != null)
            {
                WycieczkaModel selectedItem = (WycieczkaModel)listView.SelectedItems[0];
                wycieczkaViewModel.WczytajWycieczke(selectedItem);
                WyswietlOknoBraniaUdzialu();
            }

        }

        /// <summary>
        /// Wyświetla okno brania udziału w wycieczce przez przodownika
        /// </summary>
        private void WyswietlOknoBraniaUdzialu()
        {
            AlertCzyUstestniczylPrzodownikGrid.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Wyświetla podstawowy komunikat
        /// </summary>
        /// <param name="wiadomosc"></param>
        private void WyswietlKomunikat(string wiadomosc)
        {
            Message.Text = wiadomosc;
            BasicKomunikatGrid.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// odpowiada za logikę przycisku zamykającego podstawowy komunikat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_ZamknijKomunikat(object sender, RoutedEventArgs e)
        {
            BasicKomunikatGrid.Visibility = Visibility.Hidden;
            if (previousGridToClose != null)
            {
                previousGridToClose.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Logika przycisku, odpowiedzialnego za nawigację do menu głownego
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_CofnijDoMenuOnClick(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Logika przycisku, który zamyka okno uczestnictwa 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_zamknijAlert(object sender, RoutedEventArgs e)
        {
            AlertCzyUstestniczylPrzodownikGrid.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Logika przycisku odpowiedzialnego za potwierdzanie odbycia wycieczki
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_potwierdz(object sender, RoutedEventArgs e)
        {
            WyswietlKomunikat("POMYŚLNIE POTWIERDZONO WYCIECZKĘ");
            AlertCzyPotwierdzaPrzodownikGrid.Visibility = Visibility.Hidden;
            wycieczkaViewModel.PotwierdzAktualnaWycieczke();
            previousGridToClose = AlertCzyUstestniczylPrzodownikGrid;
            wycieczkaViewModel.UsunObecnaWycieczkeZWyswietlania();
        }

        /// <summary>
        /// Wyświetla okno do potwierdzania wycieczki przez przodownika
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_wyswietlAlertPotwierdzania(object sender, RoutedEventArgs e)
        {
            if (wycieczkaViewModel.CzyZalogowanyPrzodownikPosiadaUprawnieniaNaCurrentWycieczke())
            {
                AlertCzyUstestniczylPrzodownikGrid.Visibility = Visibility.Hidden;
                AlertCzyPotwierdzaPrzodownikGrid.Visibility = Visibility.Visible;

            }
            else
            {
                WyswietlKomunikat("NIE POSIADASZ UPRAWNIEŃ NA TEN OBSZAR GÓRSKI");
                previousGridToClose = AlertCzyUstestniczylPrzodownikGrid;
                wycieczkaViewModel.OdrzucAktualnaWycieczke();

            }
        }


        /// <summary>
        /// Logika przycisku odpowiedzialnego za odrzucenie wycieczki
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_odrzuc(object sender, RoutedEventArgs e)
        {
            WyswietlKomunikat("POMYŚLNIE ODRZUCONO WYCIECZKĘ");
            wycieczkaViewModel.OdrzucAktualnaWycieczke();
            previousGridToClose = AlertCzyPotwierdzaPrzodownikGrid;
            wycieczkaViewModel.UsunObecnaWycieczkeZWyswietlania();
        }
    }
}