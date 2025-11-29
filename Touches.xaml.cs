using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Logique d'interaction pour Touches.xaml
    /// </summary>
    public partial class Touches : UserControl
    {
        public event EventHandler RetourParametreTouches;
        public Key txt_Gauche = Key.Left;
        public Key txt_Droite = Key.Right;
        public Key txt_Sauter = Key.Up;
        public Key txt_attaque_legere = Key.A;
        public Key txt_attaque_lourde = Key.Z;
        public Key txt_Pistolet = Key.D;
        public Key txt_Soin = Key.E;
        public Touches()
        {
            InitializeComponent();
            txtGauche.Text = txt_Gauche.ToString();
            txtDroite.Text = txt_Droite.ToString();
            txtSauter.Text = txt_Sauter.ToString();
            txtAttaqueLegere.Text = txt_attaque_legere.ToString();
            txtAttaqueLourde.Text = txt_attaque_lourde.ToString();
            txtSoin.Text = txt_Soin.ToString();
        }

        private void TxtGauche_KeyDown(object sender, KeyEventArgs e)
        {
            txt_Gauche = e.Key;
            txtGauche.Text = e.Key.ToString();
        }
        private void TxtDroite_KeyDown(object sender, KeyEventArgs e)
        {
            txt_Droite = e.Key;
            txtDroite.Text = e.Key.ToString();
        }
        private void TxtSauter_KeyDown(object sender, KeyEventArgs e)
        {
            txt_Sauter = e.Key;
            txtSauter.Text = e.Key.ToString();
        }
        private void TxtAttaqueLegere_KeyDown(object sender, KeyEventArgs e)
        {
            txt_attaque_legere = e.Key;
            txtAttaqueLegere.Text = e.Key.ToString();
        }
        private void TxtAttaqueLourde_KeyDown(object sender, KeyEventArgs e)
        {
            txt_attaque_lourde = e.Key;
            txtAttaqueLourde.Text = e.Key.ToString();
        }
        private void TxtSoin_KeyDown(object sender, KeyEventArgs e)
        {
            txt_Soin = e.Key;
            txtSoin.Text = e.Key.ToString();
        }

        private void RetourParametreTouches_click(object sender, RoutedEventArgs e)
        {
            RetourParametreTouches?.Invoke(this, EventArgs.Empty);
        }

        public void TransmettreTouches(Niveau1 niveau1)
        {
            /*niveau1.ToucheGauche = txt_Gauche;
            niveau1.ToucheDroite = txt_Droite;
            niveau1.ToucheSauter = txt_Sauter;
            niveau1.ToucheAttaqueLegere = txt_attaque_legere;
            niveau1.ToucheAttaqueLourde = txt_attaque_lourde;
            niveau1.TouchePistolet = txt_Pistolet;
            niveau1.ToucheSoin = txt_Soin;*/
        }
    }


}
