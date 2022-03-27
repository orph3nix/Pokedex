using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Linq;

namespace Pokedex
{
    class Program
    {
        public static List<Pokemon> pokedex = new List<Pokemon>();
        //Liste de tout les pokemons 
        public static int[,] Generation = new int[8, 2] { { 1, 151 }, { 152, 251 }, { 252, 386 }, { 387, 493 }, { 494, 649 }, { 650, 721 }, { 722, 802 }, { 803, 898 } };
        //Utile pour avoir les index de début et de fin de chaque génération
        public static String[] listeTypes = new String[] {"Normal","Water","Steel","Fire","Ground","Fairy","Psy","Fighting","Grass","Poison","Flying","Bug", "Electric","Ghost","Rock","Ice","Dragon", "Dark"};
        //Liste de tout les types différents de Pokémons présents

        static void Main(string[] args)
        {
            var json = new WebClient().DownloadString("https://tmare.ndelpech.fr/tps/pokemons"); //Url de la liste de tous les pokemons
            var objects = JArray.Parse(json); // On le transforme en tableau
            foreach (JObject root in objects) // On parcours chaque objet (Pokemon) du tableau
            {
               int id = (int) root.Property("id").Value; //on recupère l'id du pokemon
               Uri url = (Uri) root.Property("url").Value; //on recupère l'url du pokemon
               long lastEdit = (long)root.Property("lastEdit").Value; //on récupère la propriété lastEdit
               Pokemon pkm = new Pokemon(id, url, lastEdit); //on crée le pokemon
               pokedex.Add(pkm); //On l'ajoute au pokedex
            }
            Console.WriteLine(pokedex.Count); //Affichage du nombre de pokemon
            List<Thread> listeThread = new List<Thread>(); //Liste de Threads, utilise pour tester si ils sont en vie

            //Oui, c'est moche et long, je sais, mais j'ai pas le choix, quand j'ai voulu le faire
            //dans une boucle for, seulement 3 à 4 threads se lançent 
            Thread thread1 = new Thread(() => InitialisationPokemonGeneration(1));
            //On crée le thread avec comme procédure InitialisationPokemonGeneration, avec comme paramètre la génération
            thread1.Name ="Thread1";//On nomme le thread, pour le voir dans le debug
            listeThread.Add(thread1); // Utilise pour attendre que tous les threads soient terminés
            thread1.Start();// On lance le thread

            Thread thread2 = new Thread(() => InitialisationPokemonGeneration(2));
            thread2.Name = "Thread2";
            listeThread.Add(thread2);
            thread2.Start();

            Thread thread3 = new Thread(() => InitialisationPokemonGeneration(3));
            thread3.Name = "Thread3";
            listeThread.Add(thread3);
            thread3.Start();

            Thread thread4 = new Thread(() => InitialisationPokemonGeneration(4));
            thread4.Name = "Thread4";
            listeThread.Add(thread4);
            thread4.Start();

            Thread thread5 = new Thread(() => InitialisationPokemonGeneration(5));
            thread5.Name = "Thread5";
            listeThread.Add(thread5);
            thread5.Start();

            Thread thread6 = new Thread(() => InitialisationPokemonGeneration(6));
            thread6.Name = "Thread6";
            listeThread.Add(thread6);
            thread6.Start();

            Thread thread7 = new Thread(() => InitialisationPokemonGeneration(7));
            thread7.Name = "Thread7";
            listeThread.Add(thread7);
            thread7.Start();

            Thread thread8 = new Thread(() => InitialisationPokemonGeneration(8));
            thread8.Name = "Thread8";
            listeThread.Add(thread8);
            thread8.Start();


            while (testIfAThreadIsStillRuning(listeThread))//on appelle la méthode pour vérifier s'ils encore un Thread est en vie
            {
                //attendre ici
                
            }
            //  main ici 
            //
            //AfficherPokedex();
            //AfficherPokedexSelonGeneration(1);
            //AfficherPokedexSelonType("Fire");
            //AfficherMoyennePoidsType("Steel");
            //AfficherUnTypeParGeneration();
        }

        static Boolean testIfAThreadIsStillRuning(List<Thread> listeThread){//On test si un des threads est encore en vie
            Boolean retour = false;
            foreach (Thread thr in listeThread) {
                retour = thr.IsAlive || retour;
            }
            return retour;
        }

        static void InitialisationPokemonGeneration(int generation) { //On initialise les pokémons selon la  génération
            if (generation < 1) //Pour éviter un "crash" si la valeur est incorrecte
            {
                generation = 1;
            }
            if (generation > 8)
            {
                generation = 8;
            }
            int indexDebut=Generation[generation-1,0];//On recupère les index de début et fin en fonction de la génération
            int indexFin= Generation[generation-1, 1];
            for (int i = indexDebut; i< indexFin+1; i++) {
                pokedex[i-1].getPokemonJson();//Procédure pour récupérer et stocker les données d'un pokémon
                Console.WriteLine("Thread " + generation + " : " + i + "/" + indexFin); //Affichage afin d'avoir le statut de l'initialisation
            }
        }

        public static void AfficherPokedex() {//Pour afficher l'entiereté du pokédex
            foreach (Pokemon pokemon in pokedex) {
                pokemon.Afficher();
            }
        }

        public static void AfficherPokedexSelonGeneration(int generation) //Pour afficher les pokemons d'une génération
        {
            if (generation < 1)//Pour éviter un "crash" si la valeur est incorrecte
            {
                generation = 1;
            }
            if (generation > 8)
            {
                generation = 8;
            }
            int indexDebut = Generation[generation - 1, 0];//On recupère les index de début et fin en fonction de la génération
            int indexFin = Generation[generation - 1, 1];
            for (int i = indexDebut; i < indexFin; i++)
            {
                pokedex[i-1].Afficher();//On affiche les pokemons
            }

        }

        public static void AfficherPokedexSelonType(String type) {//On affiche tous les pokemons comportant un type
            foreach (Pokemon pokemon in pokedex) {//On parcours tous les pokémons
                String[] types = pokemon.getTypes();
                if (types.Contains(type))//On teste si le type passé par paramètre est contenu dans les types du pokémon
                {
                    pokemon.Afficher();//On affiche les pokemons
                }
            }
        }

        public static void AfficherMoyennePoidsType(String type) {//ON affiche la moyenne de poids pour un type de Pokemon donné
            int totalPoids = 0;
            int compteur = 0;
            foreach (Pokemon pokemon in pokedex)//On parcours tous les pokémons
            {
                String[] types = pokemon.getTypes();
                if (types.Contains(type))//On teste si le type passé par paramètre est contenu dans les types du pokémon
                {
                    totalPoids = totalPoids + pokemon.getPoids();//On stocke le total des poids
                    compteur++;//On compte le total des pokémons du type
                }
            }//Affichage
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=");
            Console.WriteLine("La moyenne de poids des pokémons de type " + type + " est de : " + ((double)totalPoids / compteur));//Calcul classique d'une moyenne
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=");

        }

        public static void AfficherUnTypeParGeneration(){
            List<Pokemon> resultat = new List<Pokemon>();
            for (int gen = 0; gen < 8; gen++) {//Une itération par génération
                List<String> listeTypesModifiable = new List<String>(listeTypes);// A chaque génération, on re initialise la liste
                for (int index = Generation[gen, 0]; index < Generation[gen, 1]; index++) {// On récupère les index de début et fin selon la génération
                    Pokemon pkm = pokedex[index]; //on récupère le pokémon
                    foreach (String type in pkm.getTypes()) {//Comme un pokémon peut avoir plusieur type, on les parcourent tous
                        if (listeTypesModifiable.Contains(type) && !resultat.Contains(pkm)) { //on teste que le type ne soit pas déja présent et on évite les doublons
                            resultat.Add(pkm); //On ajoute le pokémon dans résultat
                            listeTypesModifiable.Remove(type); //On enlève le type dans la liste
                        }
                    }
                }
            }
            foreach (Pokemon pkm in resultat) {// Affichage
                pkm.Afficher();
            }
            
        
        
        }
    }
}