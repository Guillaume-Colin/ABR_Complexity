using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace TreeImplementation
{
    class Noeud
    {
        public double Valeur;

        public Noeud Gauche;

        public Noeud Droite;

        public Noeud(int valeur)
        {
            Valeur = valeur;
            Gauche = null;
            Droite = null;
        }
    }

    class Program
    {
                static void Main(string[] args)
        {
            Console.WriteLine("Bienvenue dans le programme de gestion d'arbres binaires de recherche (ABR).");

            while (true)
            {
                Console.WriteLine("\nMenu :");
                Console.WriteLine("1. Créer un ABR complet");
                Console.WriteLine("2. Manipuler un ABR complet");
                Console.WriteLine("3. Parcours d'un ABR en mode suffixe");
                Console.WriteLine("4. Quitter");

                Console.Write("\nEntrez le numéro de l'opération que vous souhaitez exécuter : ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        Ex_2_1_5_TestComplet();
                        break;
                    case "2":
                        Console.WriteLine("Fonctionnalité non implémentée pour le moment.");
                        break;
                    case "3":
                        int iteration = 0;
                        double duree = 0;
                        Noeud arbre = Ex_2_1_2_CreerABRComplet(4, ref duree);
                        List<double> parcoursSuffixe = ParcoursSuffixe(arbre, 0, ref iteration);
                        Console.WriteLine("\nParcours en mode suffixe :");
                        Console.WriteLine(String.Join(",", parcoursSuffixe));
                        break;
                    case "4":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Choix invalide. Veuillez entrer un numéro valide.");
                        break;
                }
            }
        }

        static Noeud Ex_2_1_1_InsererNoeud(ref Noeud arbre, int valeurAInserer)
        {
            //Thread.Sleep(100);

            if (arbre == null)
            {
                return new Noeud(valeurAInserer);
            }
            else
            {
                if (valeurAInserer <= arbre.Valeur)
                {
                    arbre.Gauche = Ex_2_1_1_InsererNoeud(ref arbre.Gauche, valeurAInserer);
                    return arbre;
                }
                else
                {
                    arbre.Droite = Ex_2_1_1_InsererNoeud(ref arbre.Droite, valeurAInserer);
                    return arbre;
                }
            }
        }

        static double[] Ex_2_1_2_a_InitialiserTableauABRComplet(int p)
        {
            // Initialisation du tableau T des valeurs à insérer dans l'ABR
            int n = (int)(Math.Pow(2, p + 1) - 1);
            double[] T = new double[n + 1];
            T[1] = Math.Pow(2, p);
            int k = 2;
            for (int i = p - 1; i >= 0; i--)
            {
                T[k] = Math.Pow(2, i);
                k++;
                for (int j = 1; j <= Math.Pow(2, p - i) - 1; j++)
                {
                    T[k] = T[k - 1] + Math.Pow(2, i + 1);
                    k++;
                }
            }
            return T;
        }

        static Noeud Ex_2_1_2_CreerABRComplet(int p, ref double duree)
        {
            double[] T = Ex_2_1_2_a_InitialiserTableauABRComplet(p);

            DateTime debut = DateTime.Now;

            // Parcours du tableau T et insertion dans l'ABR
            Noeud arbre = null;
            for (int i = 1; i <= T.Length - 1; i++)
            {
                arbre = Ex_2_1_1_InsererNoeud(ref arbre, (int)T[i]);
            }

            DateTime fin = DateTime.Now;
            duree = (fin - debut).TotalMilliseconds;
            Console.WriteLine(string.Format("Arbre complet créé, profondeur={0}, nombre d'éléments={1}, temps de traitement : {2}ms, (Tick debut={3}, Ticks fin={4})", p, T.Length, Math.Round(duree), debut.Ticks, fin.Ticks));

            return arbre;
        }

        static double[] Ex_2_1_3_a_InitialiserTableauABRFiliformeCroissant(int p)
        {
            // Initialisation du tableau T des valeurs à insérer dans l'ABR
            int n = (int)(Math.Pow(2, p + 1) - 1);
            double[] T = new double[n + 1];
            for (int i = 1; i <= T.Length - 1; i++)
            {
                T[i] = i;
            }
            return T;
        }

        static Noeud Ex_2_1_3_b_CreerArbreFiliforme(int p, ref double duree)
        {
            double[] T = Ex_2_1_3_a_InitialiserTableauABRFiliformeCroissant(p);
            DateTime debut = DateTime.Now;

            // Parcours du tableau T et insertion dans l'ABR
            Noeud arbre = null;
            for (int i = 1; i <= T.Length - 1; i++)
            {
                arbre = Ex_2_1_1_InsererNoeud(ref arbre, (int)T[i]);
            }

            DateTime fin = DateTime.Now;
            duree = (fin - debut).TotalMilliseconds;
            Console.WriteLine(string.Format("Arbre filiforme créé, profondeur={0}, nombre d'éléments={1}, temps de traitement : {2}ms, (Tick debut={3}, Ticks fin={4})", p, T.Length, Math.Round(duree), debut.Ticks, fin.Ticks));

            return arbre;
        }

        static double Ex_2_1_4(int p)
        {
            Noeud arbre;
            double dureeArbreComplet = 0;

            double duree = 0;
            arbre = Ex_2_1_2_CreerABRComplet(p, ref duree);
            dureeArbreComplet = duree;

            duree = 0;
            arbre = Ex_2_1_3_b_CreerArbreFiliforme(p, ref duree);

            return dureeArbreComplet;
        }

        static void Ex_2_1_5_TestComplet()
        {
            Console.WriteLine("Exercice 2.1");

            int p = 1;
            double dureeArbreComplet = 0;

            while (dureeArbreComplet < 180000 && p <= 12)
            {
                dureeArbreComplet = Ex_2_1_4(p);
                p++;
            }
        }

        /// <summary>
        /// Parcours d'un ABR en mode SUFFIXE
        /// </summary>
        /// <param name="arbre"></param>
        /// <param name="iteration"></param>
        /// <returns></returns>
        static List<double> ParcoursSuffixe(Noeud arbre, int profondeur, ref int iteration)
        {
            List<double> parcours = new List<double>();

            if (arbre != null)
            {
                profondeur++;
                parcours.AddRange(ParcoursSuffixe(arbre.Gauche, profondeur, ref iteration));
                parcours.AddRange(ParcoursSuffixe(arbre.Droite, profondeur, ref iteration));
                parcours.Add(arbre.Valeur);

                Console.WriteLine(string.Format("Iteration={0}, Profondeur={1}, Valeur du noeud={2}", iteration++, profondeur, arbre.Valeur));
            }

            return parcours;
        }

        /// <summary>
        /// Parcours d'un ABR en mode INFIXE
        /// </summary>
        /// <param name="arbre"></param>
        /// <param name="iteration"></param>
        /// <returns></returns>
        static List<double> ParcoursInfixe(Noeud arbre, int profondeur, ref int iteration)
        {
            List<double> parcours = new List<double>();

            if (arbre != null)
            {
                profondeur++;
                parcours.AddRange(ParcoursInfixe(arbre.Gauche, profondeur, ref iteration));
                parcours.Add(arbre.Valeur);
                parcours.AddRange(ParcoursInfixe(arbre.Droite, profondeur, ref iteration));

                Console.WriteLine(string.Format("Iteration={0}, Profondeur={1}, Valeur du noeud={2}", iteration++, profondeur, arbre.Valeur));
            }

            return parcours;
        }

        /// <summary>
        /// Parcours d'un ABR en mode PREFIXE
        /// </summary>
        /// <param name="arbre"></param>
        /// <param name="iteration"></param>
        /// <returns></returns>
        static List<double> ParcoursPrefixe(Noeud arbre, int profondeur, ref int iteration)
        {
            List<double> parcours = new List<double>();

            if (arbre != null)
            {
                profondeur++;
                parcours.Add(arbre.Valeur);
                parcours.AddRange(ParcoursPrefixe(arbre.Gauche, profondeur, ref iteration));
                parcours.AddRange(ParcoursPrefixe(arbre.Droite, profondeur, ref iteration));

                Console.WriteLine(string.Format("Iteration={0}, Profondeur={1}, Valeur du noeud={2}", iteration++, profondeur, arbre.Valeur));
            }

            return parcours;
        }

        static Noeud Ex_2_2_1_RechercherABR(Noeud arbre, double valeur)
        {
            Noeud arbreTrouve = null;

            if (arbre != null)
            {
                if (arbre.Valeur == valeur)
                    arbreTrouve = arbre;
                else
                {
                    if (valeur < arbre.Valeur)
                        arbreTrouve = Ex_2_2_1_RechercherABR(arbre.Gauche, valeur);
                    else
                        arbreTrouve = Ex_2_2_1_RechercherABR(arbre.Droite, valeur);
                }
            }
            return arbreTrouve;
        }

        static void Ex_2_2_1_SupprimerMax(ref Noeud arbre, ref double y)
        {
            if (arbre.Droite == null)
            {
                y = arbre.Valeur;
                arbre = arbre.Gauche;
            }
            else
            {
                Ex_2_2_1_SupprimerMax(ref arbre.Droite, ref y);
            }
        }

        static void Ex_2_2_1_SupprimerABR(ref Noeud arbre, double x)
        {
            double y = 0;
            if (arbre != null)
            {
                if (x < arbre.Valeur)
                    Ex_2_2_1_SupprimerABR(ref arbre.Gauche, x);
                else
                {
                    if (x > arbre.Valeur)
                        Ex_2_2_1_SupprimerABR(ref arbre.Droite, x);
                    else
                    {
                        if (arbre.Gauche == null)
                            arbre = arbre.Droite;
                        else
                        {
                            if (arbre.Droite == null)
                                arbre = arbre.Gauche;
                            else
                            {
                                Ex_2_2_1_SupprimerMax(ref arbre.Gauche, ref y);
                                arbre.Valeur = y;
                            }
                        }
                    }
                }
            }
        }

        static Noeud Ex_2_2_2_ManipulerABRComplet(Noeud arbre)
        {
            int p = ProfondeurABR(arbre);

            for (double i = Math.Pow(2, p); i == 1; i--)
            {
                Ex_2_2_1_SupprimerABR(ref arbre, i);
                Ex_2_1_1_InsererNoeud(ref arbre, (int)i);
            }
            return arbre;
        }

        static void Ex_2_2_3_RechercheValeur_1(int p)
        {
            double duree = 0;
            Noeud A = Ex_2_1_2_CreerABRComplet(p, ref duree);
            Noeud Aprime = Ex_2_2_2_ManipulerABRComplet(A);
            Ex_2_2_1_RechercherABR(A, 1);
            Ex_2_2_1_RechercherABR(Aprime, 1);
        }

        static void Ex_2_2_4_TestsComplets()
        {
            for (int p = 1; p <= 12; p++)
            {
                Ex_2_2_3_RechercheValeur_1(p);
            }
        }

        static void DumpArbre(Noeud arbre, int p)
        {
            if (arbre == null)
                return;

            string valeurGauche = arbre.Gauche != null ? arbre.Gauche.Valeur.ToString() : "null";
            string valeurDroite = arbre.Droite != null ? arbre.Droite.Valeur.ToString() : "null";

            Console.WriteLine("Niveau {0}, Noeud {1}, Fils gauche={2}, Fils Droite={3}", p, arbre.Valeur, valeurGauche, valeurDroite);

            DumpArbre(arbre.Gauche, p + 1);
            DumpArbre(arbre.Droite, p + 1);
        }

        /// <summary>
        /// Retourne la profondeur d'un ABR
        /// </summary>
        /// <param name="arbre"></param>
        /// <returns></returns>
        static int ProfondeurABR(Noeud arbre)
        {
            if (arbre == null)
                return 0;

            if (arbre.Gauche == null && arbre.Droite == null)
                return 0;

            int profondeurGauche = ProfondeurABR(arbre.Gauche);
            int profondeurDroite = ProfondeurABR(arbre.Droite);
            return 1 + Math.Max(profondeurGauche, profondeurDroite);
        }

        static void SiftUp(double[] T, int i)
        {
            bool fini = false;
            if (i != 1)
            {
                do
                {
                    int j = PartieEntiere(i / 2);
                    if (T[i] > T[j])
                    {
                        double tempValue = T[i];
                        T[i] = T[j];
                        T[j] = tempValue;
                    }
                    else
                    {
                        fini = true;
                    }
                    i = j;
                }
                while (!(i == 1 || fini));
            }
        }

        static void SiftDown(double[] T, int i)
        {
            int n = T.Length - 1;
            bool fini = false;
            if (2 * i <= n)
                do
                {
                    i = 2 * i;
                    if (i + 1 <= n && T[i + 1] > T[i])
                        i = i + 1;
                    int j = PartieEntiere(i / 2);
                    if (T[j] < T[i])
                    {
                        double tempValue = T[i];
                        T[i] = T[j];
                        T[j] = tempValue;
                    }
                    else
                        fini = true;
                }
                while (!(2 * i > n || fini));
        }

        static int PartieEntiere(decimal i)
        {
            return (int)(Math.Floor(i));
        }

        static double[] InsererTB(double[] T, double x)
        {
            int n = T.Length;
            Array.Resize(ref T, n + 1);
            T[n] = x;
            SiftUp(T, n);
            return T;
        }

        //static void SupprimerTB(double[] T, int i)
        //{
        //    double x = T[i];
        //    double y = T[T.Length];
        //    int = T.Length - 1;
        //}


        /// <summary>
        /// Initialise un tableau de n= 2**p+1 -1 element de facon aleatoire
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        static double[] InitRandomTB(int p)
        {
            int n = (int)Math.Pow(2, p + 1) - 1;

            List<double> list = new List<double>();
            for (int i = 1; i <= n; i++)
                list.Add(i);

            double[] T = new double[n + 1];
            Random random = new Random(DateTime.Now.Millisecond);
            for (int i = 1; i <= n; i++)
            {
                int rnd = random.Next(0, list.Count);
                T[i] = list[rnd];
                list.RemoveAt(rnd);
            }
            //Console.WriteLine(string.Format("InitRandomTB({0})={1}", p, string.Join(", ", T)));

            return T;

        }

        static double[] CreerTB(double[] T, ref double duree)
        {
            DateTime debut = DateTime.Now;
            double[] maxTasBinaire = new double[1];
            for (int i = 1; i <= T.Length - 1; i++)
            {
                maxTasBinaire = InsererTB(maxTasBinaire, i);
            }
            DateTime fin = DateTime.Now;
            duree = (fin - debut).TotalMilliseconds;

            //Console.WriteLine(string.Format("CreerTB={0}, dureeTotale={1}", string.Join(", ", maxTasBinaire), duree));
            return maxTasBinaire;
        }

        static void Ex_2_3_Question_3(int p)
        {
            Console.WriteLine("Exercice 2.3 Question 3");

            double dureeTotale = 0;
            double dureeAuMieux = double.MaxValue;
            double dureeAuPire = 0;
            while (dureeTotale <= 180000)
            {
                double[] T = InitRandomTB(p);
                double duree = 0;
                double[] maxTasBinaire = CreerTB(T, ref duree);
                dureeTotale = dureeTotale + duree;
                Console.WriteLine("dureeTotale=" + dureeTotale);
                if (duree < dureeAuMieux) dureeAuMieux = duree;
                if (duree > dureeAuPire) dureeAuPire = duree;
            }
            Console.WriteLine(string.Format("Exercice 2.3 Question 3, profondeur={0}, durée au mieux={1}ms, durée au pire={2}ms", p, dureeAuMieux, dureeAuPire));


        }

        static void Ex_2_3_Question_4(int p)
        {
            Console.WriteLine("Exercice 2.3 Question 4");

            double dureeTotale = 0;
            double dureeAuMieux = double.MaxValue;
            double dureeAuPire = 0;
            while (dureeTotale <= 180000)
            {
                double[] T = InitRandomTB(p);
                double duree = 0;
                double[] maxTasBinaire = CreerTB(T, ref duree);
                RechercherValeur1_TB(maxTasBinaire, ref duree);
                dureeTotale = dureeTotale + duree;
                Console.WriteLine("dureeTotale=" + dureeTotale);
                if (duree < dureeAuMieux) dureeAuMieux = duree;
                if (duree > dureeAuPire) dureeAuPire = duree;
            }
            Console.WriteLine(string.Format("Exercice 2.3 Question 4, profondeur={0}, durée au mieux={1}ms, durée au pire={2}ms", p, dureeAuMieux, dureeAuPire));


        }

        static void RechercherValeur1_TB(double[] maxTasBinaire, ref double duree)
        {
            DateTime debut = DateTime.Now;
            DateTime fin = DateTime.Now;
            duree = 0;
            int i = 0;
            while (i <= maxTasBinaire.Length - 1)
            {
                if (maxTasBinaire[i] == 1)
                {
                    fin = DateTime.Now;
                    i = maxTasBinaire.Length;
                }
                i++;
            }
            duree = (fin - debut).TotalMilliseconds;
            return;
        }

        static void Ex_2_3_5_TestComplet(int p)
        {
            Console.WriteLine("Exercice 2.3 Question 4");

            double dureeTotale = 0;
            double dureeAuMieux = double.MaxValue;
            double dureeAuPire = 0;
            while (dureeTotale <= 180000)
            {
                double[] T = InitRandomTB(p);
                double duree = 0;
                double[] maxTasBinaire = CreerTB(T, ref duree);
                SupprimerMax_TB(ref maxTasBinaire, ref duree);
                dureeTotale = dureeTotale + duree;
                Console.WriteLine("dureeTotale=" + dureeTotale);
                if (duree < dureeAuMieux) dureeAuMieux = duree;
                if (duree > dureeAuPire) dureeAuPire = duree;
            }
            Console.WriteLine(string.Format("Exercice 2.3 Question 4, profondeur={0}, durée au mieux={1}ms, durée au pire={2}ms", p, dureeAuMieux, dureeAuPire));
        }

        static void SupprimerMax_TB(ref double[] maxTasBinaire, ref double duree)
        {
            DateTime debut = DateTime.Now;
            DateTime fin = DateTime.Now;
            duree = 0;
            int i = 0;
            while (i <= maxTasBinaire.Length - 1)
            {
                if (maxTasBinaire[i] == 1)
                {
                    fin = DateTime.Now;
                    i = maxTasBinaire.Length;
                }
                i++;
            }
            duree = (fin - debut).TotalMilliseconds;
            return;
        }

        static void Ex_2_3_5_TestComplet()
        {
            for (int p = 1; p <= 12; p++)
            {
                Ex_2_3_5_TestComplet(p);
            }
        }
    }
}
