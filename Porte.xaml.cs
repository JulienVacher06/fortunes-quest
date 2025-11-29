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
    /// Logique d'interaction pour Porte.xaml
    /// </summary>
    public partial class Porte : UserControl
    {
        private static string[] ENIGME = new string[] { "Je suis le meilleur pirate que le monde ait jamais porté, qui suis-je ?", "j'entre dur et sec, et je ressors mou et mouillé, qui suis-je ?", "plus il y a de moi, moins vous me voyez" };
        private static string[,] REPONSE_ENIGME = new string[,] { { "Benoît de Diard", "Barbe noir", "Jack Sparrow" }, { "un phare", "la poudre noire", "un biscuit dans un thé" }, { "les mutins", "les ténèbres", "des trésors" } };
        private static Random ALEATOIRE = new Random();
        private static int ENIGME_ALEATOIRE = 0;
        private static string enigme, reponse1, reponse2, reponse3 = " ";
        public event EventHandler finDuJeuGagne;
        public event EventHandler finDuJeuPerd;

        public Porte()
        {
            InitializeComponent();
        }
        private void but_suivant_Click(object sender, RoutedEventArgs e)
        {
            TB_parole.Text = ENIGME[ENIGME_ALEATOIRE];
            TB_rep1.Text = REPONSE_ENIGME[ENIGME_ALEATOIRE, 0];
            TB_rep2.Text = REPONSE_ENIGME[ENIGME_ALEATOIRE, 1];
            TB_rep3.Text = REPONSE_ENIGME[ENIGME_ALEATOIRE, 2];
        }

        

        private static string Enigme(string enigme)
        {
            ENIGME_ALEATOIRE = ALEATOIRE.Next(0, 2);
            enigme = ENIGME[ENIGME_ALEATOIRE];

            return enigme;
        }
        private void BT_rep1_Click(object sender, RoutedEventArgs e)
        {
            if (TB_parole.Text == ENIGME[0])
                finDuJeuGagne?.Invoke(this, EventArgs.Empty);
            else
                finDuJeuPerd?.Invoke(this, EventArgs.Empty);

        }

        public void BT_rep2_Click(object sender, RoutedEventArgs e)
        {
            if (TB_parole.Text == ENIGME[2])
                finDuJeuGagne?.Invoke(this, EventArgs.Empty);
            else
                finDuJeuPerd?.Invoke(this, EventArgs.Empty);
        }
        public void BT_rep3_Click(object sender, RoutedEventArgs e)
        {
            if (TB_parole.Text == ENIGME[1])
                finDuJeuGagne?.Invoke(this, EventArgs.Empty);
            else
                finDuJeuPerd?.Invoke(this, EventArgs.Empty);
        }
    }
}
