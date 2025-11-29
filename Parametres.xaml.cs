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
    /// Logique d'interaction pour Parametres.xaml
    /// </summary>
    public partial class Parametres : UserControl
    {
        public UserControl Son;
        public UserControl Touches;

        public event EventHandler RetourParametreClick;
        public event EventHandler ParametreToucheClick;
        public event EventHandler<double> ChangementVolume_Musique;
        public Parametres()
        {
            InitializeComponent();
        }

        private void btn_Parametres_Touches(object sender, RoutedEventArgs e)
        {
            ParametreToucheClick?.Invoke(this, new EventArgs());
        }

        private void RetourParametre_click(object sender, RoutedEventArgs e)
        {
            RetourParametreClick?.Invoke(this, EventArgs.Empty);
        }

        private void Changement_Valeur(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ChangementVolume_Musique?.Invoke(this, e.NewValue);
        }
    }
}
