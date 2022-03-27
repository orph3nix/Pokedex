using Newtonsoft.Json.Linq;
using System;
using System.Net;

namespace Pokedex
{
    class Pokemon
    {
        protected int id;//id du pokemon
        protected Uri url;//url des infos du pokemon
        protected long lastEdit;//propriété lastEdit
        protected String[] types;//tableau des types du Pokemon
        protected String nom;//nom du pokemon
        protected String description;//description du pokemon
        protected int poids;//poids du pokemon

        //J'ai fait le choix de récupérer seulement les infos des pokémons qui nous intéressent pour l'exercice
        public Pokemon(int id, Uri url, long lastEdit) {//Constructeur d'un Pokémon
            this.id = id;
            this.url = url;
            this.lastEdit = lastEdit;
        }

        public void getPokemonJson() {//Procédure pour récupérer les données du Pokemon
            var json = new WebClient().DownloadString(this.url);//On récupère le json à l'Url du pokemon
            var obj = JObject.Parse(json);//On le convertit en tableau Json
            this.types = Newtonsoft.Json.JsonConvert.DeserializeObject<String[]>(obj.Property("types").Value.ToString());//On récupère les types comme un tableau de chaines de caractères
            JObject noms = (JObject)obj.Property("name").Value;//On récupère l'attribut name
            this.nom = noms.Property("fr").Value.ToString();//On récupère l'attribut fr de name
            JObject descriptions = (JObject)obj.Property("description").Value;//On récupère l'attribut description
            this.description = descriptions.Property("fr").Value.ToString();//On récupère l'attribut fr de description
            this.poids = (int) obj.Property("weight").Value; //On récupère l'attribut weight
        }

        public void Afficher() {//Affichage du Pokemon (id et nom) 
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=");
            Console.WriteLine("Id : "+id);
            Console.WriteLine("Nom : " + nom);
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=");
        }
        public String[] getTypes() {//Retourne les types du pokemon
            return this.types;
        }

        public int getPoids() {//Retourne le poids du pokemon
            return this.poids;    
        }
    }
}
