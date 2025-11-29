using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Shapes;


namespace SAE
{
    public partial class Niveau1 : UserControl
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



        private BitmapImage[,] animationImmobileEnnemi;
        private BitmapImage[,] animationAttaqueGaucheEnnemi;
        private BitmapImage[,] animationAttaqueDroitEnnemi;
        private BitmapImage[,] animationDeplacementGaucheEnnemi;
        private BitmapImage[,] animationDeplacementDroitEnnemi;
        private BitmapImage[,] animationMortEnnemi;
        private BitmapImage[,] animationToucheEnnemi;

        private Image[] imageEnnemis;

        private DispatcherTimer timerJeu;
        private DispatcherTimer timerAnimation;

        private bool droite, gauche, sautEnCours;
        private bool attaqueLourdeCapitaine, attaqueLegereCapitaine, seSoigner, pouvoirSeSoigner = false;
        private double angleSaut;
        private double positionYInitial;
        public bool passerNiveauSuivant { get; set; } = false;
        private bool ennemiPeutBouger = true;

        private readonly string CHEMIN_BALLE_CAPITAINE = "pack://application:,,,/images/Personnage/1_personnage/tir/capitaine_balle.png";
        private readonly string CHEMIN_CARTE = "pack://application:,,,/images/Bonus/carte.png";
        private readonly string CHEMIN_RHUM = "pack://application:,,,/images/Bonus/rhum.png";

        private bool[] deplacementEnnemiGauche;
        private bool[] deplacementEnnemiDroite;
        private bool[] attaqueGaucheEnnemi;
        private bool[] attaqueDroitEnnemi;
        private bool[] deplacementEnnemiSaut;
        private bool[] mortEnnemi;
        private bool balleEnCoursCapitaine;

        private Rectangle rectangleJoueur;
        private Rectangle[] rectanglesEnnemis;
        private Rectangle rectanglePotion;
        private Rectangle rectangleCarte;

        private int[] indexEnnemi;
        private int NBImmobile, NBDroit, NBGauche, NBAttaqueLourde, NBAttaqueLourdeGauche, NBAttaqueLegere, NBAttaqueLegereGauche, NBToucheCapitaine, NBMortCapitaine = 0;
        private int NBSoin = 0;
        private int degatSubit = 0, indexCoeur, coeurAAffiche = 0;

        private int NBDroitEnnemi, NBAttaqueDroitEnnemi, NBGaucheEnnemi, NBAttaqueGaucheEnnemi, NBMortEnnemi, NBToucheEnnemi = 0;
        private int NBImmobileEnnemi, indexTypeEnnemi = 0;
        private int degatBoss = 20;

        private int degatSubitParEnnemi, degatTotalSubit = 0;

        

        private readonly int TEMPS_ANIMATION = 5 * 16;
        private readonly int VITESSE_PERSO_PRINCIPAL = 5;
        private readonly double SAUT_HAUTEUR = 210;
        private readonly double SAUT_VITESSE = 0.1;
        private readonly int NOMBRE_ENNEMIS_NIVEAU1 = 1;
        private readonly int[] DEGAT_CAPITAINE = [ 25, 15 ];

        private readonly int DISTANCE_ATTAQUE = 30;
        private readonly int DISTANCE_DEPLACEMENT = 300;
        private readonly int PV_ENNEMI_INITIAL = 600;
        private readonly double PV_CAPITAINE = 200;
        private readonly double VITESSE_ENNEMI = 3;

        public event EventHandler NiveauSuivant;
        public event EventHandler CapitaineEstMort;
        public event EventHandler CheatCode;

        public Niveau1()
        {
            InitializeComponent();

            InitialisationAnnimationImageEnnemi();
            InitialisationAnimationImagesCapitaine();

            InitialisationTimerAnimation();
            InitialisationTimerJeu();
            InitialiserEnnemis();
            InitialiserTousLesRectangle();

            this.KeyDown += Niveau1_KeyDown;
            this.KeyUp += Niveau1_KeyUp;
            pv_Ennemi_Niveau1.Text = PV_ENNEMI_INITIAL.ToString();

#if DEBUG
            Console.WriteLine(droite);
            Console.WriteLine(gauche);
            Console.WriteLine(sautEnCours);
            Console.WriteLine(attaqueLegereCapitaine);
            Console.WriteLine(attaqueLourdeCapitaine);
            Console.WriteLine(attaqueDroitEnnemi);
            Console.WriteLine(seSoigner);
            Console.WriteLine(mortEnnemi);
            Console.WriteLine(angleSaut);
            Console.WriteLine(animationAttaqueDroitEnnemi);
            Console.WriteLine(animationAttaqueGaucheEnnemi);
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

            CanvaNiveau1.Children.Add(rectangleImage);
            Canvas.SetLeft(rectangleImage, Canvas.GetLeft(image));
            Canvas.SetTop(rectangleImage, Canvas.GetTop(image));
        }

        public void InitialiserTousLesRectangle ()
        {
            //rectangle joueur
            InitialiserRectangle(capitaine, ref rectangleJoueur);
            //rectangle potion
            InitialiserRectangle(potion, ref rectanglePotion);

            InitialiserRectangle(bonusAAttrape[0], ref rectangleCarte);
        }


        //initialisation de toutes les images du jeu
        public void InitialisationAnimationImagesCapitaine()
        {
            //test
            bonusAAttrape = new Image[2];

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
            coeursAfficheCapitaine = new Image[5] { coeur1, coeur2, coeur3, coeur4, coeur5 };

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

           

        }
        private void InitialisationAnnimationImageEnnemi ()
        {
            // ANIMATION ENNEMI
            animationImmobileEnnemi = new BitmapImage[1, 4];
            for (int i = 0; i < animationImmobileEnnemi.GetLength(0); i++)
            {
                for (int j = 0; j < animationImmobileEnnemi.GetLength(1); j++)
                {
                    string chemin = $"pack://application:,,,/images/Niveau1/Mechants/mini_boss_inv/immobile/immobile{j + 1}.png";
                    animationImmobileEnnemi[i, j] = new BitmapImage(new Uri(chemin));
                }
            }

            animationAttaqueGaucheEnnemi = new BitmapImage[1, 6];
            for (int i = 0; i < animationAttaqueGaucheEnnemi.GetLength(0); i++)
            {
                int limite = animationAttaqueGaucheEnnemi.GetLength(1);

                if (i == 2)
                    limite = 3;

                for (int j = 0; j < limite; j++)
                {
                    string chemin = $"pack://application:,,,/images/Niveau1/Mechants/mini_boss_inv/attaque2/attaque2{j + 1}.png";
                    animationAttaqueGaucheEnnemi[i, j] = new BitmapImage(new Uri(chemin));
                }
            }

            animationAttaqueDroitEnnemi = new BitmapImage[1, 6];
            for (int i = 0; i < animationAttaqueDroitEnnemi.GetLength(0); i++)
            {
                int limite = animationAttaqueDroitEnnemi.GetLength(1);

                if (i == 2)
                    limite = 3;

                for (int j = 0; j < limite; j++)
                {
                    string chemin = $"pack://application:,,,/images/Niveau1/Mechants/mini_boss/attaque2/attaque2{j + 1}.png";
                    animationAttaqueDroitEnnemi[i, j] = new BitmapImage(new Uri(chemin));
                }
            }


            animationDeplacementGaucheEnnemi = new BitmapImage[1, 6];
            for (int i = 0; i < animationDeplacementGaucheEnnemi.GetLength(0); i++)
            {
                for (int j = 0; j < animationDeplacementGaucheEnnemi.GetLength(1); j++)
                {
                    string chemin = $"pack://application:,,,/images/Niveau1/Mechants/mini_boss_inv/marche/marche{j + 1}.png";
                    animationDeplacementGaucheEnnemi[i, j] = new BitmapImage(new Uri(chemin));
                }
            }

            animationDeplacementDroitEnnemi = new BitmapImage[1, 6];
            for (int i = 0; i < animationDeplacementDroitEnnemi.GetLength(0); i++)
            {
                for (int j = 0; j < animationDeplacementDroitEnnemi.GetLength(1); j++)
                {
                    string chemin = $"pack://application:,,,/images/Niveau1/Mechants/mini_boss/marche/marche{j + 1}.png";
                    animationDeplacementDroitEnnemi[i, j] = new BitmapImage(new Uri(chemin));
                }
            }

            animationMortEnnemi = new BitmapImage[1, 5];
            for (int i = 0; i < animationMortEnnemi.GetLength(0); i++)
            {
                for (int j = 0; j < animationMortEnnemi.GetLength(1); j++)
                {
                    string chemin = $"pack://application:,,,/images/Niveau1/Mechants/mini_boss/mort/mort{j + 1}.png";
                    animationMortEnnemi[i, j] = new BitmapImage(new Uri(chemin));
                }
            }

            animationToucheEnnemi = new BitmapImage[1, 2];
            for (int i = 0; i < animationToucheEnnemi.GetLength(0); i++)
            {
                for (int j = 0; j < animationToucheEnnemi.GetLength(1); j++)
                {
                    string chemin = $"pack://application:,,,/images/Niveau1/Mechants/mini_boss/touche/touche{j + 1}.png";
                    animationToucheEnnemi[i, j] = new BitmapImage(new Uri(chemin));
                }
            }
        }
        //fin initialisation images


        //initialisation pour permettre d'avoir un nombre d'ennemi déterminé, énoncé dans la variable NOMBRE_ENNEMI_NIVEAU1
        private void InitialiserEnnemis()
        {
            imageEnnemis = new Image[NOMBRE_ENNEMIS_NIVEAU1];
            rectanglesEnnemis = new Rectangle[NOMBRE_ENNEMIS_NIVEAU1];
            indexEnnemi = new int[NOMBRE_ENNEMIS_NIVEAU1];
            Random random = new Random();

            for (int i = 0; i < NOMBRE_ENNEMIS_NIVEAU1; i++)
            {
                Image ennemi = new Image();
                Rectangle rectangleEnnemi = new Rectangle
                {
                    Width = capitaine.Width,
                    Height = capitaine.Height,
                    Fill = Brushes.Transparent,
                    Stroke = Brushes.Transparent,
                    StrokeThickness = 1
                };

                int indexEnnemi = random.Next(animationImmobileEnnemi.GetLength(0));
                ennemi.Source = animationImmobileEnnemi[indexEnnemi, 0];

                double positionX = random.Next(0, 1080);
                double positionY = Canvas.GetTop(capitaine);

                Canvas.SetLeft(ennemi, positionX);
                Canvas.SetTop(ennemi, positionY);

                Canvas.SetLeft(rectangleEnnemi, positionX);
                Canvas.SetTop(rectangleEnnemi, positionY);

                ennemi.Width = capitaine.Width;
                ennemi.Height = capitaine.Height;

                CanvaNiveau1.Children.Add(ennemi);
                CanvaNiveau1.Children.Add(rectangleEnnemi);

                imageEnnemis[i] = ennemi;
                rectanglesEnnemis[i] = rectangleEnnemi;
            }

            deplacementEnnemiDroite = new bool[imageEnnemis.Length];
            deplacementEnnemiGauche = new bool[imageEnnemis.Length];
            attaqueGaucheEnnemi = new bool[imageEnnemis.Length];
            attaqueDroitEnnemi = new bool[imageEnnemis.Length];
            mortEnnemi = new bool[imageEnnemis.Length];
            
            //degatSubitParEnnemi = new int[imageEnnemis.Length];
        }
        //fin de la methode


        //gestion des appuies et relachement de touche
        public void Niveau1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                droite = true;
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
            if(e.Key == Key.P)
            {
                CheatCode?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Niveau1_KeyUp(object sender, KeyEventArgs e)
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
        private bool CollisionEntreCapitaineEtPotion ()
        {
            if(CollisionEntreDeuxElement(rectangleJoueur, rectanglePotion) )
            {
                pouvoirSeSoigner = true;
                CanvaNiveau1.Children.Remove(potion);
                CanvaNiveau1.Children.Remove(rectanglePotion);
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
            AnimationEnnemis();
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
            if (ennemiPeutBouger)
            {
                for (int i = 0; i < imageEnnemis.Length; i++)
                {
                    DeterminationDistanceJoueurEnnemis(i);
                    DeplacementEnnemiVersJoueur(i);
                } 
            }
            CollisionEntreCapitaineEtPotion();


            if (mortEnnemi[0])
            {
                if (CollisionEntreDeuxElement(rectangleJoueur, rectangleCarte))
                {
                    passerNiveauSuivant = true;
                    NiveauSuivant?.Invoke(this, EventArgs.Empty);
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
            if (CollisionEntreDeuxElement(rectangleJoueur, rectanglesEnnemis[0]) && attaqueGaucheEnnemi[0] && ennemiPeutBouger)
            {
                NBToucheCapitaine++;
                if (NBToucheCapitaine == animationToucheCapitaine.Length)
                    NBToucheCapitaine = 0;
                capitaine.Source = animationToucheCapitaine[NBToucheCapitaine];

                ChangementImageCoeurSelonDegatSubit(degatBoss);
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
                    pv_Ennemi_Niveau1.Text = PV_ENNEMI_INITIAL.ToString();

                    ennemiPeutBouger = false;
                    


                    for (int i = 0; i < coeursAfficheCapitaine.Length; i++)
                    {
                        coeursAfficheCapitaine[i].Source = coeursCapitaine[0];
                    }
                    
                    CapitaineEstMort?.Invoke(this, new EventArgs());
                    degatSubit = 0;
                    Focusable = false;
                }
                ennemiPeutBouger = true;


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
        private void AnimationEnnemis()
        {
            if (ennemiPeutBouger)
            {
                for (int i = 0; i < imageEnnemis.Length; i++)
                {
                    int typeEnnemi = i % animationImmobileEnnemi.GetLength(0);

                    // Animation immobile
                    AnimerDeuxDimensionSansBooleen(ref indexEnnemi[i], imageEnnemis[i], animationImmobileEnnemi, typeEnnemi);

                    // Animation Déplacement Gauche
                    if (deplacementEnnemiGauche[i])
                        AnimerDeuxDimensionSansBooleen(ref NBGaucheEnnemi, imageEnnemis[i], animationDeplacementGaucheEnnemi, typeEnnemi);


                    // Animation Déplacement Droit
                    if (deplacementEnnemiDroite[i])
                        AnimerDeuxDimensionSansBooleen(ref NBDroitEnnemi, imageEnnemis[i], animationDeplacementDroitEnnemi, typeEnnemi);


                    // Animation Attaque Gauche
                    if (attaqueGaucheEnnemi[i])
                        AnimerDeuxDimensionSansBooleen(ref NBAttaqueGaucheEnnemi, imageEnnemis[i], animationAttaqueGaucheEnnemi, typeEnnemi);

                    // Animation Attaque Droite
                    if (attaqueDroitEnnemi[i])
                        AnimerDeuxDimensionSansBooleen(ref NBAttaqueDroitEnnemi, imageEnnemis[i], animationAttaqueDroitEnnemi, typeEnnemi);

                    // Animation Mort Ennemi
                    if (mortEnnemi[i])
                    {
                        
                        // Si l'animation de mort n'est pas terminée
                        if (NBMortEnnemi < animationMortEnnemi.GetLength(1) - 1)
                        {
                            NBMortEnnemi++;
                            imageEnnemis[i].Source = animationMortEnnemi[typeEnnemi, NBMortEnnemi];
                        }
                        else
                        {
                            double positionX = Canvas.GetLeft(imageEnnemis[i]);
                            double positionY = Canvas.GetTop(imageEnnemis[i]);

                            CanvaNiveau1.Children.Remove(imageEnnemis[i]);

                            Canvas.SetLeft(bonusAAttrape[0], positionX);
                            Canvas.SetTop(bonusAAttrape[0], positionY);

                            //bonusAAttrape[0].Visibility = Visibility.Visible;

                            CanvaNiveau1.Children.Add(bonusAAttrape[0]);

                            ennemiPeutBouger = false;

                            InitialiserRectangle(bonusAAttrape[0], ref rectangleCarte);
                        }
                    }
                    else
                    {
                        // Mettre à jour la position de l'image cachée pour qu'elle suive l'ennemi vivant
                        double positionX = Canvas.GetLeft(imageEnnemis[i]);
                        double positionY = Canvas.GetTop(imageEnnemis[i]);
                        Canvas.SetLeft(bonusAAttrape[0], positionX);
                        Canvas.SetTop(bonusAAttrape[0], positionY);
                    }

                    if ((attaqueLourdeCapitaine || attaqueLegereCapitaine) && CollisionEntreDeuxElement(rectangleJoueur, rectanglesEnnemis[i]))
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
                        degatSubitParEnnemi += degatTemporaire;

                        degatTotalSubit += degatTemporaire;
                        int pvActuelleEnnemi = PV_ENNEMI_INITIAL - degatTotalSubit;
                        pv_Ennemi_Niveau1.Text = pvActuelleEnnemi.ToString();

                        NBToucheEnnemi++;
                        if (NBToucheEnnemi >= animationToucheEnnemi.GetLength(1))
                        {
                            NBToucheEnnemi = 0;
                        }

                        Console.WriteLine(degatSubitParEnnemi); // Affiche les dégâts totaux
                        imageEnnemis[i].Source = animationToucheEnnemi[typeEnnemi, NBToucheEnnemi];

                        if (degatSubitParEnnemi >= PV_ENNEMI_INITIAL)
                        {
                            mortEnnemi[i] = true;
                        }
                        attaqueLourdeCapitaine = false;
                        attaqueLegereCapitaine = false;
                    }
                }

            }
        }


        //test
        public void DeterminationDistanceJoueurEnnemis(int indiceEnnemi)
        {
            double PositionXEnnemi = Canvas.GetLeft(imageEnnemis[indiceEnnemi]);
            double PositionXCapitaine = Canvas.GetLeft(capitaine);

            /*double PositionYEnnemi = Canvas.GetTop(imageEnnemis[indiceEnnemi]);
            double PositionYCapitaine = Canvas.GetTop(capitaine);*/

            double differenceDePositionX = PositionXEnnemi - PositionXCapitaine;
            /*double differenceDePositionY = PositionYEnnemi - PositionYCapitaine;*/

            if (differenceDePositionX > 0 && differenceDePositionX < DISTANCE_DEPLACEMENT)
            {
                deplacementEnnemiGauche[indiceEnnemi] = true;
                deplacementEnnemiDroite[indiceEnnemi] = false;
            }
            else if (differenceDePositionX > 0 && differenceDePositionX > DISTANCE_DEPLACEMENT)
            {
                deplacementEnnemiGauche[indiceEnnemi] = false;
            }

            if (differenceDePositionX < 0 && differenceDePositionX > -DISTANCE_DEPLACEMENT)
            {
                deplacementEnnemiDroite[indiceEnnemi] = true;
                deplacementEnnemiGauche[indiceEnnemi] = false;
            }
            else if (differenceDePositionX < 0 && differenceDePositionX < -DISTANCE_DEPLACEMENT)
            {
                deplacementEnnemiDroite[indiceEnnemi] = false;
            }

            if (differenceDePositionX > 0 && differenceDePositionX < DISTANCE_ATTAQUE)
            {
                attaqueGaucheEnnemi[indiceEnnemi] = true;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            
                attaqueDroitEnnemi[indiceEnnemi] = false;
            }
            else if (differenceDePositionX > 0 && differenceDePositionX > DISTANCE_ATTAQUE)
            {
                attaqueGaucheEnnemi[indiceEnnemi] = false;
            }
            if (differenceDePositionX < 0 && differenceDePositionX > -DISTANCE_ATTAQUE)
            {
                attaqueDroitEnnemi[indiceEnnemi] = true;
                attaqueGaucheEnnemi[indiceEnnemi] = false;
            }
            else if (differenceDePositionX < 0 && differenceDePositionX < -DISTANCE_ATTAQUE)
            {
                attaqueDroitEnnemi[indiceEnnemi] = false;
            }

        }

        public void DeplacementEnnemiVersJoueur(int indiceEnnemi)
        {
            if (deplacementEnnemiDroite[indiceEnnemi])
            {
                Canvas.SetLeft(imageEnnemis[indiceEnnemi], Canvas.GetLeft(imageEnnemis[indiceEnnemi]) + VITESSE_ENNEMI);
                Canvas.SetLeft(rectanglesEnnemis[indiceEnnemi], Canvas.GetLeft(imageEnnemis[indiceEnnemi]));
            }
            if (deplacementEnnemiGauche[indiceEnnemi])
            {
                Canvas.SetLeft(imageEnnemis[indiceEnnemi], Canvas.GetLeft(imageEnnemis[indiceEnnemi]) - VITESSE_ENNEMI);
                Canvas.SetLeft(rectanglesEnnemis[indiceEnnemi], Canvas.GetLeft(imageEnnemis[indiceEnnemi]));
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
    }
}
