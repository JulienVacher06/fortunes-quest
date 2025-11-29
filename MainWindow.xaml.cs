using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SAE
{
    public partial class MainWindow : Window
    {
        public UserControl accueil;
        public Niveau1 niveau1;
        public UserControl parametres;
        public UserControl butDuJeu;
        public Touches touches;
        public UserControl porte;
        public UserControl victoire;
        public UserControl defaite;
        public UserControl niveauFinal;
        public UserControl transition;

        private static MediaPlayer musiqueFond;
        private static MediaPlayer musiqueniveau1;
        private static MediaPlayer musiquePortes;
        private static MediaPlayer musiquePerdu;
        private static MediaPlayer musiqueGagne;
        private static MediaPlayer musiqueBoss;
        private readonly string SON_PERDU = "son/rire_mort.mp3";
        private readonly string SON_ACCUEIL = "son/musique_accueil.mp3";
        private readonly string SON_GAGNE = "son/musique_gagne.mp3";
        private readonly string SON_NIVEAU1 = "son/musique_de_fond_1.mp3";
        private readonly string SON_BOSS = "son/musique_boss_final.mp3";
        private readonly string SON_GAME_OVER = "son/son_game_over.mp3";

        public static double volumeMusiqueFond = 0.5;
        public static double NiveauSon { get; set; }
        public static int menuPrincipal { get; set; }
        public bool JeuTermime { get; set; } = false;
        public MainWindow()
        {
            InitializeComponent();
            InitMusique();
            accueil = new Accueil();
            niveau1 = new Niveau1();
            parametres = new Parametres();
            butDuJeu = new butDuJeu();
            touches = new Touches();
            porte = new Porte();
            victoire = new Victoire();
            defaite = new Defaite();
            niveauFinal = new NiveauFinal();
            transition = new Transition();

            AfficherAccueil();

            GestionUCAccueil();
            GestionParametreEtButDuJeu();
            GestionParametreToucheEtSon();
            PorteAGagne();
            GestionGagneOuPerd();
            GestionNiveau1();
            GestionNiveauFinal();
            TransitionAuBoss();
        }
       
    
        private void AfficherVictoireCheatCodeNiveau1(object sender, EventArgs e)
        {
            ChangementScene(victoire);
            InitMusiqueGagne(musiqueniveau1);
        }

        private void PorteAGagne()
        {
            ((Porte)porte).finDuJeuGagne += AfficherVictoire;
            ((Porte)porte).finDuJeuPerd += AfficherDefaite;
        }
        private void AfficherDefaite(object sender, EventArgs e)
        {
            ChangementScene(defaite);
            InitMusiquePerdu(musiquePortes);
        }
        private void AfficherVictoire(object sender, EventArgs e)
        {
            ChangementScene(victoire);
            InitMusiqueGagne(musiquePortes);
        }
        private void GestionGagneOuPerd()
        {
            ((Defaite)defaite).retourMenuPrincipalPerdu += AfficherAccueilDepuisFin;
            ((Victoire)victoire).retourMenuPrincipalGagne += AfficherAccueilDepuisFinGagne;
        }

        private void GestionNiveau1()
        {
            ((Niveau1)niveau1).NiveauSuivant += AfficherTransition;
            ((Niveau1)niveau1).CapitaineEstMort += AfficherDefaiteDepuisNiveau1;
            ((Niveau1)niveau1).CheatCode += AfficherVictoireCheatCodeNiveau1;

        }
        private void GestionNiveauFinal()
        {
            ((NiveauFinal)niveauFinal).CheatCodeNiveauFinal += AfficherVictoireCheatCodeFinal;
            ((NiveauFinal)niveauFinal).CapitaineEstMortNiveauFinal += AfficherDefaiteDepuisNiveauFinal;
            ((NiveauFinal)niveauFinal).PasserAuxPortes += AfficherPortesDepuisNiveauFinal;
        }

        private void AfficherPortesDepuisNiveauFinal (object sender, EventArgs e)
        {
            ChangementScene(porte);
            InitMusiquePortes();
        }

        private void AfficherDefaiteDepuisNiveauFinal(object sender, EventArgs e)
        {
            ChangementScene(defaite);
            InitMusiquePerdu(musiqueBoss);
        }
        private void AfficherVictoireCheatCodeFinal(object sender, EventArgs e)
        {
            ChangementScene(victoire);
            InitMusiqueGagne(musiqueBoss);
        }
        private void AfficherTransition(object sender, EventArgs e)
        {
            ChangementScene(transition);
        }
        private void TransitionAuBoss()
        {
            ((Transition)transition).TransitionAuBoss += AfficherNiveauFinal;
        }
        private void AfficherNiveauFinal(object sender, EventArgs e)
        {
            ChangementScene(niveauFinal);
            InitMusiqueBoss();
        }

        private void AfficherAccueilDepuisFin(object sender, EventArgs e)
        {
            ChangementScene(accueil);
            InitMusique();
        }
        private void AfficherAccueilDepuisFinGagne(object sender, EventArgs e)
        {
            ChangementScene(accueil);
            musiqueGagne.Stop();
            InitMusique();
        }
        private void GestionUCAccueil()
        {
            ((Accueil)accueil).AppuyerSurJouer += AfficherNiveau1;
            ((Accueil)accueil).AppuyerSurParametre += AfficherParametre;
            ((Accueil)accueil).AppuyerSurButDuJeu += AfficherButDuJeu;
        }
        //

        private void GestionParametreEtButDuJeu()
        {
            ((Parametres)parametres).ParametreToucheClick += AfficherParametreTouche;
            ((Parametres)parametres).RetourParametreClick += AfficherAccueilDepuisParametre;
            ((butDuJeu)butDuJeu).RetourParametreButDuJeu += AfficherAccueilDepuisParametre;
        }

        private void GestionParametreToucheEtSon()
        {
            ((Touches)touches).RetourParametreTouches += AfficherParametre;
            ((Parametres)parametres).ChangementVolume_Musique += ChangementVolume;
        }

        private void ChangementVolume(object sender, double value)
        {
            volumeMusiqueFond = value / 10;
            musiqueFond.Volume = volumeMusiqueFond;
            Console.WriteLine(volumeMusiqueFond);
        }

        private void AfficherAccueil()
        {
            ChangementScene(accueil);
        }

        private void AfficherNiveau1(object sender, EventArgs e)
        {
            ChangementScene(niveau1);
            InitMusiqueNiveau1();
        }

        private void AfficherParametre(object sender, EventArgs e)
        {
            ChangementScene(parametres);
        }

        private void AfficherButDuJeu(object sender, EventArgs e)
        {
            ChangementScene(butDuJeu);
        }

        private void AfficherParametreTouche(object sender, EventArgs e)
        {
            ChangementScene(touches);
        }

        private void AfficherAccueilDepuisParametre(object sender, EventArgs e)
        {
            ChangementScene(accueil);
        }

        private void ChangementScene(UserControl userControl)
        {
            Main_Container.Children.Clear();
            Main_Container.Children.Add(userControl);

            userControl.Loaded += (s, e) =>
            {
                Keyboard.Focus(userControl);
            };
        }


        private void InitMusique()
        {
            musiqueFond = new MediaPlayer();
            musiqueFond.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + SON_ACCUEIL));
            musiqueFond.MediaEnded += RelanceMusique;
            musiqueFond.Volume = volumeMusiqueFond;
            musiqueFond.Play();
        }

        private void RelanceMusique(object? sender, EventArgs e)
        {
            musiqueFond.Position = TimeSpan.Zero;
            musiqueFond.Play();
        }

        private void InitMusiqueBoss()
        {
            musiqueBoss = new MediaPlayer();
            musiqueBoss.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + SON_BOSS));
            musiqueBoss.MediaEnded += RelanceMusiqueBoss;
            musiqueBoss.Volume = volumeMusiqueFond;
            musiqueBoss.Play();
            musiqueniveau1.Stop();
        }

        private void RelanceMusiqueBoss(object? sender, EventArgs e)
        {
            musiqueBoss.Position = TimeSpan.Zero;
            musiqueBoss.Play();
        }

        private void InitMusiqueNiveau1()
        {
            musiqueniveau1 = new MediaPlayer();
            musiqueniveau1.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + SON_NIVEAU1));
            musiqueniveau1.MediaEnded += RelanceMusiqueNiveau1;
            musiqueniveau1.Volume = volumeMusiqueFond;
            musiqueniveau1.Play();
            musiqueFond.Stop();
        }

        private void RelanceMusiqueNiveau1(object? sender, EventArgs e)
        {
            musiqueniveau1.Position = TimeSpan.Zero;
            musiqueniveau1.Play();
        }

        private void InitMusiquePortes()
        {
            musiquePortes = new MediaPlayer();
            musiquePortes.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + SON_ACCUEIL));
            musiquePortes.MediaEnded += RelanceMusique;
            musiquePortes.Volume = volumeMusiqueFond;
            musiquePortes.Play();
            musiqueBoss.Stop();
        }

        private void InitMusiquePerdu(MediaPlayer musiqueAArretee)
        {
            musiquePerdu = new MediaPlayer();
            musiquePerdu.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + SON_PERDU));
            musiquePerdu.Volume = volumeMusiqueFond;
            musiqueAArretee.Stop();
            musiquePerdu.Play();
        }

        private void InitMusiqueGagne(MediaPlayer musiqueAArretee)
        {
            musiqueGagne = new MediaPlayer();
            musiqueGagne.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + SON_GAGNE));
            musiqueGagne.MediaEnded += RelanceMusiqueGagne;
            musiqueGagne.Volume = volumeMusiqueFond;
            musiqueAArretee.Stop();
            musiqueGagne.Play();
        }

        private void RelanceMusiqueGagne(object? sender, EventArgs e)
        {
            musiqueGagne.Position = TimeSpan.Zero;
            musiqueGagne.Play();
        }


        private void AfficherDefaiteDepuisNiveau1(object sender, EventArgs e)
        {
            ChangementScene(defaite);
            InitMusiquePerdu(musiqueniveau1);
        }
    }
}
