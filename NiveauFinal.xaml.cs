using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SAE
{
    /// <summary>
    /// Logique d'interaction pour NiveauFinal.xaml
    /// </summary>
    public partial class NiveauFinal : UserControl
    {
        
        private BitmapImage[] animationImmobileCapitaine;
        private BitmapImage[] animationDroiteCapitaine;
        private BitmapImage[] animationGaucheCapitaine;
        private BitmapImage[] animationToucheCapitaine;
        private BitmapImage[] animationMortCapitaine;
        private BitmapImage[] animationSoinCapitaine;
        private BitmapImage[,] animationAttaqueDroiteCapitaine;
        private BitmapImage[,] animationAttaqueGaucheCapitaine;
        private BitmapImage[] animationTir;
        private Image balleCapitaine;
        private Image[] bonusAAttrape;
        private BitmapImage[] coeursCapitaine;
        private Image[] coeursAfficheCapitaine;



        private BitmapImage[,] animationImmobileBoss;
        private BitmapImage[,] animationAttaqueGaucheBoss;
        private BitmapImage[,] animationAttaqueDroitBoss;
        private BitmapImage[,] animationDeplacementGaucheBoss;
        private BitmapImage[,] animationDeplacementDroitBoss;
        private BitmapImage[,] animationMortBoss;
        private BitmapImage[,] animationToucheBoss;

        private Image[] imagePlateforme;

        private DispatcherTimer timerJeu;
        private DispatcherTimer timerAnimation;

        private bool droite, gauche, sautEnCours;
        private bool attaqueLourdeCapitaine, attaqueLegereCapitaine, seSoigner, pouvoirSeSoigner = false;
        private double angleSaut;
        private double positionYInitial;
        public bool passerNiveauSuivant { get; set; } = false;
        private bool bossPeutBouger = true;

        private readonly string CHEMIN_BALLE_CAPITAINE = "pack://application:,,,/images/Personnage/1_personnage/tir/capitaine_balle.png";
        private readonly string CHEMIN_CARTE = "pack://application:,,,/images/Bonus/carte.png";
        private readonly string CHEMIN_RHUM = "pack://application:,,,/images/Bonus/rhum.png";

        private bool deplacementBossGauche;
        private bool deplacementBossDroite;
        private bool attaqueGaucheBoss;
        private bool attaqueDroitBoss;
        private bool deplacementBossSaut;
        private bool mortBoss;
        private bool balleEnCoursCapitaine;

        private Rectangle rectangleJoueur;
        private Rectangle rectangleBoss;
        private Rectangle rectanglePotion;
        private Rectangle rectanglePotion2;
        private Rectangle rectangleCarte;
        private Rectangle[] rectanglePlateforme;

        private int indexBoss;
        private int NBImmobile, NBDroit, NBGauche, NBAttaqueLourde, NBAttaqueLourdeGauche, NBAttaqueLegere, NBAttaqueLegereGauche, NBToucheCapitaine, NBMortCapitaine = 0;
        private int NBSoin = 0;
        private int degatSubit = 0, indexCoeur, coeurAAffiche = 0;

        private int NBDroitBoss, NBAttaqueDroitBoss, NBGaucheBoss, NBAttaqueGaucheBoss, NBMortBoss, NBToucheBoss = 0;
        private int NBImmobileBoss, indexTypeBoss = 0;
        private int DEGAT_BOSS = 20;

        private int degatSubitParBoss, degatTotalSubitBoss = 0;

        private readonly int TEMPS_ANIMATION = 5 * 16;
        private readonly int VITESSE_PERSO_PRINCIPAL = 5;
        private readonly double SAUT_HAUTEUR = 210;
        private readonly double SAUT_VITESSE = 0.1;
        private readonly int NOMBRE_ENNEMIS_NIVEAU1 = 1;
        private readonly int[] DEGAT_CAPITAINE = [25, 15];

        private readonly int DISTANCE_ATTAQUE = 20;
        private readonly int DISTANCE_DEPLACEMENT = 600;
        private readonly int PV_BOSS_INITIAL = 2000;
        private readonly double PV_CAPITAINE = 200;

        public event EventHandler CheatCodeNiveauFinal;
        public event EventHandler BossAPortes;
        public event EventHandler CapitaineEstMortNiveauFinal;
        public event EventHandler PasserAuxPortes;

        public NiveauFinal()
        {
            InitializeComponent();
            InitialisationAnimationImages();
            InitialisationTimerAnimation();
            InitialisationTimerJeu();
            InitialiserTousLesRectangle();

            this.KeyDown += NiveauFinal_KeyDown;
            this.KeyUp += NiveauFinal_KeyUp;

            pv_Ennemi_NiveauFinal.Text = PV_BOSS_INITIAL.ToString();

#if DEBUG
            Console.WriteLine(droite);
            Console.WriteLine(gauche);
            Console.WriteLine(attaqueDroitBoss);
            Console.WriteLine(attaqueGaucheBoss);
            Console.WriteLine(attaqueLourdeCapitaine);
            Console.WriteLine(attaqueLegereCapitaine);
            Console.WriteLine(seSoigner);
            Console.WriteLine(bossPeutBouger);
            Console.WriteLine(degatSubit);
            Console.WriteLine(degatSubitParBoss);
            Console.WriteLine(degatTotalSubitBoss);
#endif

        }

        //Initialisation de rectangles, pour que chaque élément soit détouré d'un rectangle
        public void InitialiserRectangle(Image image, ref Rectangle rectangleImage)
        {
            rectangleImage = new Rectangle
            {
                Width = image.Width,
                Height = image.Height,
                Fill = Brushes.Transparent,
                Stroke = Brushes.Transparent,
                StrokeThickness = 1
            };

            canvaNiveauFinal.Children.Add(rectangleImage);
            Canvas.SetLeft(rectangleImage, Canvas.GetLeft(image));
            Canvas.SetTop(rectangleImage, Canvas.GetTop(image));
        }

        public void InitialiserRectangle2Dim(Image[] images, Rectangle[] rectangleImage)
        {
            rectangleImage = new Rectangle[images.Length]; // Pas besoin de 'ref' ici
            for (int i = 0; i < images.Length; i++)
            {
                rectangleImage[i] = new Rectangle
                {
                    Width = images[i].Width,
                    Height = images[i].Height,
                    Fill = Brushes.Transparent,
                    Stroke = Brushes.Transparent,
                    StrokeThickness = 1
                };

                canvaNiveauFinal.Children.Add(rectangleImage[i]);

                // Assurez-vous que l'image est ajoutée au canevas avant de récupérer ses coordonnées
                Canvas.SetLeft(rectangleImage[i], Canvas.GetLeft(images[i]));
                Canvas.SetTop(rectangleImage[i], Canvas.GetTop(images[i]));
            }
        }


        public void InitialiserTousLesRectangle()
        {
            //rectangle joueur
            InitialiserRectangle(capitaine, ref rectangleJoueur);
            //rectangle potion
            InitialiserRectangle(potion1, ref rectanglePotion);

            InitialiserRectangle(bonusAAttrape[0], ref rectangleCarte);

            InitialiserRectangle(potion2, ref rectanglePotion2);

            InitialiserRectangle(dragon, ref rectangleBoss);

            imagePlateforme = new Image[] { plateforme1, plateforme2, plateforme3, plateforme4, plateforme5, plateforme6, plateforme7, plateforme8, plateforme9 };
            InitialiserRectangle2Dim(imagePlateforme, rectanglePlateforme);

        }


        //initialisation de toutes les images du jeu
        public void InitialisationAnimationImages()
        {
            //test
            bonusAAttrape = new Image[2];

            //Stocker Images des plateformes dans un tableau
             

            // Initialisation des images
            bonusAAttrape[0] = new Image
            {
                Width = 60,
                Height = 100,
                Source = new BitmapImage(new Uri(CHEMIN_CARTE, UriKind.RelativeOrAbsolute)),
            };

            bonusAAttrape[1] = new Image
            {
                Width = 50,
                Height = 50,
                Source = new BitmapImage(new Uri(CHEMIN_RHUM, UriKind.RelativeOrAbsolute)),
            };


            coeursCapitaine = new BitmapImage[3];
            for (int i = 0; i < coeursCapitaine.Length; i++)
            {
                coeursCapitaine[i] = new BitmapImage(new Uri($"pack://application:,,,/images/Personnage/Coeur/coeur_{i + 1}.png"));
            }
            coeursAfficheCapitaine = new Image[5] { coeurf1, coeurf2, coeurf3, coeurf4, coeurf5 };

            animationImmobileCapitaine = new BitmapImage[4];
            for (int i = 0; i < animationImmobileCapitaine.Length; i++)
            {
                string chemin = $"pack://application:,,,/images/Personnage/1_personnage/immobile/capitaine_immobile{i + 1}.png";
                string cheminPirate2 = $"pack://application:,,,/images/Niveau1/Mechants/mini_boss/immobile/immobile{i + 1}.png";
                animationImmobileCapitaine[i] = new BitmapImage(new Uri(chemin));
            }

            animationToucheCapitaine = new BitmapImage[2];
            for (int i = 0; i < animationToucheCapitaine.Length; i++)
            {
                animationToucheCapitaine[i] = new BitmapImage(new Uri($"pack://application:,,,/images/Personnage/1_personnage/capitaine_touche/capitaine_touche{i + 1}.png"));
            }

            animationMortCapitaine = new BitmapImage[6];
            for (int i = 0; i < animationMortCapitaine.Length; i++)
            {
                animationMortCapitaine[i] = new BitmapImage(new Uri($"pack://application:,,,/images/Personnage/1_personnage/mort/capitaine_mort{i + 1}.png"));
            }

            animationDroiteCapitaine = new BitmapImage[6];
            for (int i = 0; i < animationDroiteCapitaine.Length; i++)
            {
                animationDroiteCapitaine[i] = new BitmapImage(new Uri($"pack://application:,,,/images/Personnage/1_personnage/marche/capitaine_marche{i + 1}.png"));
            }

            animationGaucheCapitaine = new BitmapImage[6];
            for (int i = 0; i < animationGaucheCapitaine.Length; i++)
            {
                animationGaucheCapitaine[i] = new BitmapImage(new Uri($"pack://application:,,,/images/Personnage/1_personnage_inv/marche/inv_capitaine_marche{i + 1}.png"));
            }

            animationAttaqueDroiteCapitaine = new BitmapImage[3, 6];
            for (int i = 0; i < animationAttaqueDroiteCapitaine.GetLength(0); i++)
            {
                for (int j = 0; j < animationAttaqueDroiteCapitaine.GetLength(1); j++)
                {
                    string chemin = $"pack://application:,,,/images/Personnage/1_personnage/attaque_{i + 1}/capitaine_attaque{i + 1}{j + 1}.png";
                    animationAttaqueDroiteCapitaine[i, j] = new BitmapImage(new Uri(chemin));
                }
            }

            animationAttaqueGaucheCapitaine = new BitmapImage[3, 6];
            for (int i = 0; i < animationAttaqueGaucheCapitaine.GetLength(0); i++)
            {
                for (int j = 0; j < animationAttaqueGaucheCapitaine.GetLength(1); j++)
                {
                    string chemin = $"pack://application:,,,/images/Personnage/1_personnage_inv/attaque_{i + 1}/inv_capitaine_attaque{i + 1}{j + 1}.png";
                    animationAttaqueGaucheCapitaine[i, j] = new BitmapImage(new Uri(chemin));
                }
            }

            animationSoinCapitaine = new BitmapImage[6];
            for (int i = 0; i < animationSoinCapitaine.Length; i++)
            {
                animationSoinCapitaine[i] = new BitmapImage(new Uri($"pack://application:,,,/images/Personnage/1_personnage/sante/capitaine_sante{i + 1}.png"));
            }

            animationImmobileBoss = new BitmapImage[1, 3];
            for (int i = 0; i < animationImmobileBoss.GetLength(0); i++)
            {
                for (int j = 0; j < animationImmobileBoss.GetLength(1); j++)
                {
                    string chemin = $"pack://application:,,,/images/Niveau3/boss/Dragon_1_inv/immobile/immobile{j+1}.png";
                    animationImmobileBoss[i, j] = new BitmapImage(new Uri(chemin));
                }
            }

            animationAttaqueGaucheBoss = new BitmapImage[2, 9];
            for (int i = 0; i < animationAttaqueGaucheBoss.GetLength(0); i++)
            {
                int limite = animationAttaqueGaucheBoss.GetLength(1);

                if (i == 0)
                    limite = 4;

                for (int j = 0; j < limite; j++)
                {
                    string chemin = $"pack://application:,,,/images/Niveau3/boss/Dragon_1_inv/attaque{i+1}/attaque{i+1}{j + 1}.png";
                    animationAttaqueGaucheBoss[i, j] = new BitmapImage(new Uri(chemin));
                }
            }

            animationAttaqueDroitBoss = new BitmapImage[2, 9];
            for (int i = 0; i < animationAttaqueDroitBoss.GetLength(0); i++)
            {
                int limite = animationAttaqueDroitBoss.GetLength(1);

                if (i == 0)
                    limite = 4;

                for (int j = 0; j < limite; j++)
                {
                    string chemin = $"pack://application:,,,/images/Niveau3/boss/Dragon_1/attaque{i + 1}/attaque{i + 1}{j + 1}.png";
                    animationAttaqueDroitBoss[i, j] = new BitmapImage(new Uri(chemin));
                }
            }


            animationDeplacementGaucheBoss = new BitmapImage[1, 12];
            for (int i = 0; i < animationDeplacementGaucheBoss.GetLength(0); i++)
            {
                for (int j = 0; j < animationDeplacementGaucheBoss.GetLength(1); j++)
                {
                    string chemin = $"pack://application:,,,/images/Niveau3/boss/Dragon_1_inv/marche/marche{j + 1}.png";
                    animationDeplacementGaucheBoss[i, j] = new BitmapImage(new Uri(chemin));
                }
            }

            animationDeplacementDroitBoss = new BitmapImage[1, 9];
            for (int i = 0; i < animationDeplacementDroitBoss.GetLength(0); i++)
            {
                for (int j = 0; j < animationDeplacementDroitBoss.GetLength(1); j++)
                {
                    string chemin = $"pack://application:,,,/images/Niveau3/boss/Dragon_1/marche/marche{j + 1}.png";
                    animationDeplacementDroitBoss[i, j] = new BitmapImage(new Uri(chemin));
                }
            }

            animationMortBoss = new BitmapImage[1, 3];
            for (int i = 0; i < animationMortBoss.GetLength(0); i++)
            {
                for (int j = 0; j < animationMortBoss.GetLength(1); j++)
                {
                    string chemin = $"pack://application:,,,/images/Niveau3/boss/Dragon_1/mort/mort{j + 1}.png";
                    animationMortBoss[i, j] = new BitmapImage(new Uri(chemin));
                }
            }

            animationToucheBoss = new BitmapImage[1, 4];
            for (int i = 0; i < animationToucheBoss.GetLength(0); i++)
            {
                for (int j = 0; j < animationToucheBoss.GetLength(1); j++)
                {
                    string chemin = $"pack://application:,,,/images/Niveau3/boss/Dragon_1/touche/touche{j + 1}.png";
                    animationToucheBoss[i, j] = new BitmapImage(new Uri(chemin));
                }
            }

        }
        //fin initialisation images

        private void NiveauFinal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                droite = true;
                Console.WriteLine("test");
            }
            if (e.Key == Key.Left)
            {
                gauche = true;
            }
            if (e.Key == Key.Up && !sautEnCours)
            {
                DemarrerSaut();
            }
            if (e.Key == Key.Z)
            {
                attaqueLourdeCapitaine = true;
            }
            if (e.Key == Key.A)
            {
                attaqueLegereCapitaine = true;
            }

            if (e.Key == Key.E)
            {
                if (pouvoirSeSoigner)
                {
                    seSoigner = true;
                    degatSubit = 0;
                    for (int i = 0; i < coeursAfficheCapitaine.Length; i++)
                    {
                        coeursAfficheCapitaine[i].Source = coeursCapitaine[0];
                    }
                    coeurAAffiche = 0;
                    indexCoeur = 0;
                }
            }
            if (e.Key== Key.P)
            {
                CheatCodeNiveauFinal?.Invoke(this, EventArgs.Empty);
            }
        }

        private void NiveauFinal_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                droite = false;
            }
            if (e.Key == Key.Left)
            {
                gauche = false;
            }
        }

        //fin de la methode


        //gestion des appuies et relachement de touche
        public void Niveau1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        public void Niveau1_KeyUp(object sender, KeyEventArgs e)
        {
            
        }
        //fin


        //Methode de gestion de saut
        public void DemarrerSaut()
        {
            sautEnCours = true;
            angleSaut = 0;
            positionYInitial = Canvas.GetTop(capitaine);
        }
        public void Sauter()
        {
            if (angleSaut <= Math.PI)
            {
                double deplacementVertical = SAUT_HAUTEUR * Math.Sin(angleSaut);
                Canvas.SetTop(capitaine, positionYInitial - deplacementVertical);
                angleSaut += SAUT_VITESSE;
            }
            else
            {
                sautEnCours = false;
                Canvas.SetTop(capitaine, positionYInitial);
            }
        }
        //fin

        //methode de detection de collision
        private bool CollisionEntreCapitaineEtPotion()
        {
            if (CollisionEntreDeuxElement(rectangleJoueur, rectanglePotion))
            {
                pouvoirSeSoigner = true;
                canvaNiveauFinal.Children.Remove(potion1);
                canvaNiveauFinal.Children.Remove(rectanglePotion);
            }
            if ( CollisionEntreDeuxElement(rectangleJoueur, rectanglePotion2))
            {
                pouvoirSeSoigner = true;
                canvaNiveauFinal.Children.Remove(potion2);
                canvaNiveauFinal.Children.Remove(rectanglePotion2);
            }
            return pouvoirSeSoigner;
        }

        //methode de gestion des déplacement du joueur : saut, droite et gauche
        public void DeplacementJoueur()
        {
            double xPosition = Canvas.GetLeft(capitaine);

            if (droite && xPosition < Application.Current.MainWindow.ActualWidth - capitaine.ActualWidth)
            {
                xPosition += VITESSE_PERSO_PRINCIPAL;
                Canvas.SetLeft(capitaine, xPosition);
            }
            if (gauche && xPosition > 0)
            {
                xPosition -= VITESSE_PERSO_PRINCIPAL;
                Canvas.SetLeft(capitaine, xPosition);
            }

            if (sautEnCours)
            {
                Sauter();
            }

            Canvas.SetLeft(rectangleJoueur, Canvas.GetLeft(capitaine));
            Canvas.SetTop(rectangleJoueur, Canvas.GetTop(capitaine));
        }
        //fin

        //timer pour l'affichage des animation pour avoir un meilleur rendu en fonction de la methode Affichage animation
        public void InitialisationTimerAnimation()
        {
            timerAnimation = new DispatcherTimer();
            timerAnimation.Interval = TimeSpan.FromMilliseconds(TEMPS_ANIMATION);
            timerAnimation.Tick += AffichageAnimation;
            timerAnimation.Start();
        }
        public void AffichageAnimation(object sender, EventArgs e)
        {
            AnimationBoss();
            ToutesLesAnimationDuPersonnage();
            
        }
        //fin du timer

        //Timer du jeu en lui meme pour avoir des déplacement plus fluide
        public void InitialisationTimerJeu()
        {
            timerJeu = new DispatcherTimer();
            timerJeu.Interval = TimeSpan.FromMilliseconds(16);
            timerJeu.Tick += Jeu;
            timerJeu.Start();
        }
        private void Jeu(object sender, EventArgs e)
        {
            DeplacementJoueur();
            if (bossPeutBouger)
            {
                    DeterminationDistanceJoueurEnnemis();
                    DeplacementEnnemiVersJoueur();
            }
            CollisionEntreCapitaineEtPotion();

            /*if (rectanglePlateforme.Length != null)
                CollisionAvecPlateforme(rectangleJoueur, rectanglePlateforme);*/

            if (mortBoss)
            {
                if (CollisionEntreDeuxElement(rectangleJoueur, rectangleCarte))
                {
                    passerNiveauSuivant = true;
                    PasserAuxPortes?.Invoke(this, EventArgs.Empty);
                }
            }


        }
        //fin 

        public void ToutesLesAnimationDuPersonnage()
        {
            AnimationDeplacement();
            AnimationAttaquePersonnage();
            AnimationTouchePersonnageEtSoin();
        }

        // Toutes les animations des differentes action du personnage
        public void AnimationTouchePersonnageEtSoin()
        {
            if (CollisionEntreDeuxElement(rectangleJoueur, rectangleBoss) && attaqueGaucheBoss && bossPeutBouger)
            {
                NBToucheCapitaine++;
                if (NBToucheCapitaine == animationToucheCapitaine.Length)
                    NBToucheCapitaine = 0;
                capitaine.Source = animationToucheCapitaine[NBToucheCapitaine];

                ChangementImageCoeurSelonDegatSubit(DEGAT_BOSS);
            }
            if (seSoigner)
            {
                NBSoin++;
                if (NBSoin == animationSoinCapitaine.Length)
                {
                    NBSoin = 0;
                    seSoigner = false;
                    pouvoirSeSoigner = false;
                }

                capitaine.Source = animationSoinCapitaine[NBSoin];
            }
            if (degatSubit > PV_CAPITAINE)
            {
                NBMortCapitaine++;
                if (NBMortCapitaine < animationMortCapitaine.Length)
                {
                    capitaine.Source = animationMortCapitaine[NBMortCapitaine];
                }
                else
                {

                    droite = false;
                    gauche = false;
                    pv_Ennemi_NiveauFinal.Text = PV_BOSS_INITIAL.ToString();

                    bossPeutBouger = false;



                    for (int i = 0; i < coeursAfficheCapitaine.Length; i++)
                    {
                        coeursAfficheCapitaine[i].Source = coeursCapitaine[0];
                    }

                    CapitaineEstMortNiveauFinal?.Invoke(this, new EventArgs());
                    degatSubit = 0;
                    Focusable = false;
                }
                bossPeutBouger = true;


            }
        }
        public void AnimationDeplacement()
        {
            if (!droite && !gauche && !sautEnCours)
                AnimerUneDimension(ref NBImmobile, capitaine, animationImmobileCapitaine);

            if (droite)
                AnimerUneDimension(ref NBDroit, capitaine, animationDroiteCapitaine);

            if (gauche)
                AnimerUneDimension(ref NBGauche, capitaine, animationGaucheCapitaine);
        }

        public void AnimationAttaquePersonnage()
        {
            if (droite)
            {
                AnimationAttaquePersonnageDroite();
            }
            if (gauche)
            {
                AnimationAttaquePersonnageGauche();
            }
            else
            {
                AnimationAttaquePersonnageDroite();
            }

        }

        public void AnimationAttaquePersonnageDroite()
        {
            if (attaqueLourdeCapitaine)
                AnimerDeuxDimension(ref NBAttaqueLourde, capitaine, animationAttaqueDroiteCapitaine, ref attaqueLourdeCapitaine, 0);

            if (attaqueLegereCapitaine)
                AnimerDeuxDimension(ref NBAttaqueLegere, capitaine, animationAttaqueDroiteCapitaine, ref attaqueLegereCapitaine, 1);
        }

        public void AnimationAttaquePersonnageGauche()
        {
            if (attaqueLourdeCapitaine)
                AnimerDeuxDimension(ref NBAttaqueLourdeGauche, capitaine, animationAttaqueGaucheCapitaine, ref attaqueLourdeCapitaine, 0);

            if (attaqueLegereCapitaine)
                AnimerDeuxDimension(ref NBAttaqueLegereGauche, capitaine, animationAttaqueGaucheCapitaine, ref attaqueLegereCapitaine, 1);
        }
        //Fin des animation du personnage

        //Changement des images des coeurs
        public void ChangementImageCoeurSelonDegatSubit(int degatRecu)
        {
            degatSubit += degatRecu;

            double pvParCoeur = PV_CAPITAINE / 5.0;
            double pvParEtat = PV_CAPITAINE / 10.0;

            if (degatSubit >= (indexCoeur * pvParCoeur) + (coeurAAffiche + 1) * pvParEtat)
            {
                if (coeurAAffiche < 2)
                {
                    coeurAAffiche++;
                }
                else
                {
                    coeurAAffiche = 0;
                    indexCoeur++;
                }

                if (indexCoeur < coeursAfficheCapitaine.Length)
                {
                    coeursAfficheCapitaine[indexCoeur].Source = coeursCapitaine[coeurAAffiche];
                }
            }
        }


        //Animation de toutes les actions des ennemis
        private void AnimationBoss()
        {
            if (bossPeutBouger)
            {
                int typeEnnemi = 0;

                // Animation immobile
                AnimerDeuxDimensionSansBooleen(ref indexBoss, dragon, animationImmobileBoss, typeEnnemi);

                // Animation Déplacement Gauche
                if (deplacementBossGauche)
                    AnimerDeuxDimensionSansBooleen(ref NBGaucheBoss, dragon, animationDeplacementGaucheBoss, typeEnnemi);

                // Animation Déplacement Droit
                if (deplacementBossDroite)
                    AnimerDeuxDimensionSansBooleen(ref NBDroitBoss, dragon, animationDeplacementDroitBoss, typeEnnemi);

                // Animation Attaque Gauche
                if (attaqueGaucheBoss)
                    AnimerDeuxDimensionSansBooleen(ref NBAttaqueGaucheBoss, dragon, animationAttaqueGaucheBoss, typeEnnemi);

                // Animation Attaque Droite
                if (attaqueDroitBoss)
                    AnimerDeuxDimensionSansBooleen(ref NBAttaqueDroitBoss, dragon, animationAttaqueDroitBoss, typeEnnemi);

                // Animation Mort Ennemi
                if (mortBoss)
                {
                    // Si l'animation de mort n'est pas terminée
                    if (NBMortBoss < animationMortBoss.GetLength(1) - 1)
                    {
                        NBMortBoss++;
                        dragon.Source = animationMortBoss[typeEnnemi, NBMortBoss];
                    }
                    else
                    {
                        double positionX = Canvas.GetLeft(dragon);
                        double positionY = Canvas.GetTop(dragon);

                        canvaNiveauFinal.Children.Remove(dragon);

                        Canvas.SetLeft(bonusAAttrape[0], positionX);
                        Canvas.SetTop(bonusAAttrape[0], positionY);

                        canvaNiveauFinal.Children.Add(bonusAAttrape[0]);

                        bossPeutBouger = false;

                        InitialiserRectangle(bonusAAttrape[0], ref rectangleCarte);
                    }
                }
                else
                {
                    // Mettre à jour la position de l'image cachée pour qu'elle suive l'ennemi vivant
                    double positionX = Canvas.GetLeft(dragon);
                    double positionY = Canvas.GetTop(dragon);
                    Canvas.SetLeft(bonusAAttrape[0], positionX);
                    Canvas.SetTop(bonusAAttrape[0], positionY);
                }

                // Vérifier si le joueur attaque et si une collision se produit
                if ((attaqueLourdeCapitaine || attaqueLegereCapitaine) && CollisionEntreDeuxElement(rectangleJoueur, rectangleBoss))
                {
                    int degatTemporaire = 0;

                    if (attaqueLourdeCapitaine)
                    {
                        degatTemporaire = DEGAT_CAPITAINE[0];
                    }
                    else if (attaqueLegereCapitaine)
                    {
                        degatTemporaire = DEGAT_CAPITAINE[1];
                    }

                    // Applique les dégâts une seule fois
                    degatSubitParBoss += degatTemporaire;

                    degatTotalSubitBoss += degatTemporaire;
                    int pvActuelleEnnemi = PV_BOSS_INITIAL - degatTotalSubitBoss;
                    pv_Ennemi_NiveauFinal.Text = pvActuelleEnnemi.ToString();

                    NBToucheBoss++;
                    if (NBToucheBoss >= animationToucheBoss.GetLength(1))
                    {
                        NBToucheBoss = 0;
                    }

                    Console.WriteLine(degatSubitParBoss); // Affiche les dégâts totaux
                    dragon.Source = animationToucheBoss[typeEnnemi, NBToucheBoss];

                    if (degatSubitParBoss >= PV_BOSS_INITIAL)
                    {
                        mortBoss = true;
                    }
                    attaqueLourdeCapitaine = false;
                    attaqueLegereCapitaine = false;
                }
            }
        }



        //test
        public void DeterminationDistanceJoueurEnnemis()
        {
            double PositionXEnnemi = Canvas.GetLeft(dragon);
            double PositionXCapitaine = Canvas.GetLeft(capitaine);

            double differenceDePositionX = PositionXEnnemi - PositionXCapitaine;

            if (differenceDePositionX > 0 && differenceDePositionX < DISTANCE_DEPLACEMENT)
            {
                deplacementBossGauche = true;
                deplacementBossDroite = false;
            }
            else if (differenceDePositionX > 0 && differenceDePositionX > DISTANCE_DEPLACEMENT)
            {
                deplacementBossGauche = false;
            }

            if (differenceDePositionX < 0 && differenceDePositionX > -DISTANCE_DEPLACEMENT)
            {
                deplacementBossDroite = true;
                deplacementBossGauche = false;
            }
            else if (differenceDePositionX < 0 && differenceDePositionX < -DISTANCE_DEPLACEMENT)
            {
                deplacementBossDroite = false;
            }

            if (differenceDePositionX > 0 && differenceDePositionX < DISTANCE_ATTAQUE)
            {
                attaqueGaucheBoss = true;
                attaqueDroitBoss = false;
            }
            else if (differenceDePositionX > 0 && differenceDePositionX > DISTANCE_ATTAQUE)
            {
                attaqueGaucheBoss = false;
            }
            if (differenceDePositionX < 0 && differenceDePositionX > -DISTANCE_ATTAQUE)
            {
                attaqueDroitBoss = true;
                attaqueGaucheBoss = false;
            }
            else if (differenceDePositionX < 0 && differenceDePositionX < -DISTANCE_ATTAQUE)
            {
                attaqueDroitBoss = false;
            }

        }

        public void DeplacementEnnemiVersJoueur()
        {
            if (deplacementBossDroite)
            {
                Canvas.SetLeft(dragon, Canvas.GetLeft(dragon) + 3);
                Canvas.SetLeft(rectangleBoss, Canvas.GetLeft(dragon));
            }
            if (deplacementBossGauche)
            {
                Canvas.SetLeft(dragon, Canvas.GetLeft(dragon) - 3);
                Canvas.SetLeft(rectangleBoss, Canvas.GetLeft(dragon));
            }
        }






        //METHODE A UTILISER

        //methode pour animer les images en fonction des variable passé en parametre
        public static void AnimerUneDimension(ref int compteur, Image image, BitmapImage[] tableauAnimation)
        {
            compteur++;
            if (compteur == tableauAnimation.Length)
                compteur = 0;
            image.Source = tableauAnimation[compteur];
        }

        public static void AnimerDeuxDimension(ref int compteur, Image image, BitmapImage[,] tableauAnimation, ref bool aRendreFalse, int indice)
        {
            compteur++;
            if (compteur == tableauAnimation.GetLength(1) - 1)
            {
                aRendreFalse = false;
                compteur = 0;
            }
            image.Source = tableauAnimation[indice, compteur];
        }

        public static void AnimerDeuxDimensionSansBooleen(ref int compteur, Image image, BitmapImage[,] tableauAnimation, int indice)
        {
            compteur++;
            if (compteur == tableauAnimation.GetLength(1) - 1)
            {
                compteur = 0;
            }
            image.Source = tableauAnimation[indice, compteur];
        }
        //fin

        //detection de collision
        public static bool CollisionEntreDeuxElement(Rectangle joueur, Rectangle autreElement)
        {
            Rect rectangle1 = new Rect(Canvas.GetLeft(joueur), Canvas.GetTop(joueur), joueur.Width, joueur.Height);
            Rect rectangle2 = new Rect(Canvas.GetLeft(autreElement), Canvas.GetTop(autreElement), autreElement.Width, autreElement.Height);

            return rectangle1.IntersectsWith(rectangle2);
        }

        private void CollisionAvecPlateforme (Rectangle joueur, Rectangle[] plateformes)
        {
            for (int i = 0; i < plateformes.Length; i++)
            {
                double positionTopPlateforme = Canvas.GetTop(plateformes[i]) + plateformes[i].ActualHeight;
                double positionTopPersonnge = Canvas.GetTop(capitaine) + capitaine.ActualHeight;
                
                if (positionTopPersonnge >= positionTopPlateforme && CollisionEntreDeuxElement(joueur, plateformes[i]))
                {
                    sautEnCours = false;
                }
            }
        }
    }
}


        
        
    

