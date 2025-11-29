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
    /// Logique d'interaction pour Transition.xaml
    /// </summary>
    public partial class Transition : UserControl
    {
        private static string[] blabla = new string[] { "Je vois que tu es dans une quête intrépide. \n Laisse moi t'aider dans ta quête", "tu dois te rendre dans la Grotte du Diable pour trouver le trésor", "Mais prend garde ! \n Une dangereuse créature protège le trésor", "Et tu devras faire preuve de sagesse", "A bientôt Noble étranger" };

        private static int compteClique = 0;
        public event EventHandler TransitionAuBoss;
        public Transition()
        {
            InitializeComponent();
        }

        private void B_suivantTransitions_Click(object sender, RoutedEventArgs e)
        {
            compteClique++;
            AnimationEspace();
            Console.WriteLine(compteClique);
        }
        private void AnimationEspace () 
        {
            if (compteClique <= 5)
                TB_parlote.Text = blabla[compteClique - 1];
            else 
            {
                TransitionAuBoss?.Invoke(this, new EventArgs());

            }
                
        }

        private void B_ProchainNiveau_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
