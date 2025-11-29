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
    /// Logique d'interaction pour Defaite.xaml
    /// </summary>
    public partial class Defaite : UserControl
    {
        public event EventHandler retourMenuPrincipalPerdu;
        public Defaite()
        {
            InitializeComponent();
        }
        private void B_retour_Click(object sender, RoutedEventArgs e)
        {
            retourMenuPrincipalPerdu?.Invoke(this, EventArgs.Empty);
        }
    }
}
