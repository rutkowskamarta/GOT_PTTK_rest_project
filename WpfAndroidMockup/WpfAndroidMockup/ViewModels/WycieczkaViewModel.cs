﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAndroidMockup.Models;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.ComponentModel;

namespace WpfAndroidMockup.ViewModels
{
    public class WycieczkaViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<WycieczkaModel> wycieczkiObservableCollection;
        private WycieczkaModel currentWycieczka;
        private WycieczkiContext wycieczkiContext;
        private PrzodownicyContext przodownicyContext;
                
        public ObservableCollection<WycieczkaModel> WycieczkiObservableCollection
        {
            get
            {
                return wycieczkiObservableCollection;
            }
            set
            {
                if (wycieczkiObservableCollection != value)
                {
                    wycieczkiObservableCollection = value;
                    RaisePropertyChanged("WycieczkiObservableCollection");
                }

            }

        }
        
        public WycieczkaModel CurrentWycieczka
        {
            get
            {
                return currentWycieczka;
            }
            set
            {
                if (currentWycieczka != value)
                {
                    currentWycieczka = value;
                    RaisePropertyChanged("CurrentWycieczka");
                }

            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public UserControl CurrentView;

        public WycieczkaViewModel()
        {
            wycieczkiContext = WycieczkiContext.GetInstance();
            przodownicyContext = PrzodownicyContext.GetInstance();

            TurystaModel t = new TurystaModel();
            OdznakaModel o = new OdznakaModel(ref t);
            CurrentWycieczka = new WycieczkaModel(1, ref t, ref o, "", StatusyPotwierdzenia.NIEPOTWIERDZONA);

        }

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public void LoadAllWycieczkiToObservableCollection()
        {
            WycieczkiObservableCollection = new ObservableCollection<WycieczkaModel>();
            List<WycieczkaModel> wycieczki = wycieczkiContext.GetWycieczkiZalogowanegoTurysty();

            foreach (var item in wycieczki)
            {
                WycieczkiObservableCollection.Add(item);
            }
        }

        public void WyswietlListeNiepotwierdzonychWycieczek()
        {
            WycieczkiObservableCollection = new ObservableCollection<WycieczkaModel>();
            List<WycieczkaModel> wycieczki = wycieczkiContext.GetNiepotwierdzoneWycieczkiTurysty(DaneLogowania.IdZalogowanegoTurysty);

            foreach (var item in wycieczki)
            {
                WycieczkiObservableCollection.Add(item);
            }
        }

        public void GetWycieczkiPrzodownikaDoPotwierdzenia(long nrPrzodownika)
        {
            WycieczkiObservableCollection = new ObservableCollection<WycieczkaModel>();
            List<WycieczkaModel> wycieczki = wycieczkiContext.GetWycieczkiPrzodownikaDoPotwierdzenia(DaneLogowania.NrZalogowanegoPrzodownika);

            foreach (var item in wycieczki)
            {
                WycieczkiObservableCollection.Add(item);
            }
        }

        public void SetCurrentWycieczka(WycieczkaModel wycieczka)
        {
            CurrentWycieczka = wycieczka;
        }

        public bool CzyCurrentWycieczkaPotwierdzona()
        {
            return CurrentWycieczka.CzyPotwierdzona();
        }

        public void UsunAktualnaWycieczke()
        {
            wycieczkiContext.Usun(CurrentWycieczka.Id);
            WycieczkiObservableCollection.Remove(CurrentWycieczka);
        }

        public bool CzyPrzodownikONumerzeIstnieje(long nrPrzodownika)
        {
            return przodownicyContext.Exists(nrPrzodownika);
        }

        public void ZmienStatus(long nrPrzodownika, StatusyPotwierdzenia status)
        {
            currentWycieczka.NrPrzodownika = nrPrzodownika;
            currentWycieczka.Status = status;
        }

        public void UsunObecnaWycieczkeZWyswietlania()
        {
            wycieczkiObservableCollection.Remove(CurrentWycieczka);
        }

        public void PotwierdzAktualnaWycieczke()
        {
            currentWycieczka.Status = StatusyPotwierdzenia.POTWIERDZONA;
        }

        public void OdrzucAktualnaWycieczke()
        {
            currentWycieczka.Status = StatusyPotwierdzenia.NIEPOTWIERDZONA;
        }

        public bool CzyZalogowanyPrzodownikPosiadaUprawnieniaNaCurrentWycieczke()
        {
            return przodownicyContext.CzyPosiadaUprawnieniaNaObszarGorski(DaneLogowania.NrZalogowanegoPrzodownika, CurrentWycieczka);
        }

    }
}
