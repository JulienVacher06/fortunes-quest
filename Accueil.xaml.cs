using System;
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

namespace SAE
{
    /// <summary>
    /// Logique d'interaction pour Accueil.xaml
    /// </summary>
    ///  private static MediaPlayer musiqueFond;
    public partial class Accueil : UserControl
    {
        public event EventHandler AppuyerSurJouer;
        public event EventHandler AppuyerSurParametre;
        public event EventHandler AppuyerSurButDuJeu;
        public event EventHandler EssaiEnigme;
        public event EventHandler Test;
        public Accueil()
        {
            InitializeComponent();
            
        }
        private void btn_parametre_click(object sender, RoutedEventArgs e)
        {
            AppuyerSurParametre?.Invoke(this, EventArgs.Empty);
        }
        private void btn_but_Jeu_Click(object sender, RoutedEventArgs e)
        {
            AppuyerSurButDuJeu?.Invoke(this, EventArgs.Empty);
        }
        private void btn_Jouer_Click(object sender, RoutedEventArgs e)
        {
            AppuyerSurJouer?.Invoke(this, EventArgs.Empty);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EssaiEnigme?.Invoke(this, EventArgs.Empty);
        }

        private void test(object sender, RoutedEventArgs e)
        {
            Test?.Invoke(this, EventArgs.Empty);
        }
    }
}
