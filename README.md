# Consignes (Livraison en bas du README)

- Vous êtes développeur front-end : vous devez réaliser les consignes décrites dans le chapitre [Front-end](#Front-end)

- Vous êtes développeur back-end : vous devez réaliser les consignes décrites dans le chapitre [Back-end](#Back-end) (*)

- Vous êtes développeur full-stack : vous devez réaliser les consignes décrites dans le chapitre [Front-end](#Front-end) et le chapitre [Back-end](#Back-end) (*)

(*) Afin de tester votre API, veuillez proposer une stratégie de test appropriée.

## Front-end

Le site de e-commerce d'Alten a besoin de s'enrichir de nouvelles fonctionnalités.

### Partie 1 : Shop

- Afficher toutes les informations pertinentes d'un produit sur la liste
- Permettre d'ajouter un produit au panier depuis la liste 
- Permettre de supprimer un produit du panier
- Afficher un badge indiquant la quantité de produits dans le panier
- Permettre de visualiser la liste des produits qui composent le panier.

### Partie 2

- Créer un nouveau point de menu dans la barre latérale ("Contact")
- Créer une page "Contact" affichant un formulaire
- Le formulaire doit permettre de saisir son email, un message et de cliquer sur "Envoyer"
- Email et message doivent être obligatoirement remplis, message doit être inférieur à 300 caractères.
- Quand le message a été envoyé, afficher un message à l'utilisateur : "Demande de contact envoyée avec succès".

### Bonus : 

- Ajouter un système de pagination et/ou de filtrage sur la liste des produits
- On doit pouvoir visualiser et ajuster la quantité des produits depuis la liste et depuis le panier 

## Back-end

### Partie 1

Développer un back-end permettant la gestion de produits définis plus bas.
Vous pouvez utiliser la technologie de votre choix parmi la liste suivante :

- Node.js/Express
- Java/Spring Boot
- C#/.net Core
- PHP/Symphony : Utilisation d'API Platform interdite

Un produit a les caractéristiques suivantes : 

``` typescript
class Product {
  id: number;
  code: string;
  name: string;
  description: string;
  image: string;
  category: string;
  price: number;
  quantity: number;
  internalReference: string;
  shellId: number;
  inventoryStatus: "INSTOCK" | "LOWSTOCK" | "OUTOFSTOCK";
  rating: number;
  createdAt: number;
  updatedAt: number;
}
```

Le back-end créé doit pouvoir gérer les produits dans une base de données SQL/NoSQL ou dans un fichier json.

### Partie 2

- Imposer à l'utilisateur de se connecter pour accéder à l'API.
  La connexion doit être gérée en utilisant un token JWT.  
  Deux routes devront être créées :
  * [POST] /account -> Permet de créer un nouveau compte pour un utilisateur avec les informations fournies par la requête.   
    Payload attendu : 
    ```
    {
      username: string,
      firstname: string,
      email: string,
      password: string
    }
    ```
  * [POST] /token -> Permet de se connecter à l'application.  
    Payload attendu :  
    ```
    {
      email: string,
      password: string
    }
    ```
    Une vérification devra être effectuée parmi tout les utilisateurs de l'application afin de connecter celui qui correspond aux infos fournies. Un token JWT sera renvoyé en retour de la reqûete.
- Faire en sorte que seul l'utilisateur ayant le mail "admin@admin.com" puisse ajouter, modifier ou supprimer des produits. Une solution simple et générique devra être utilisée. Il n'est pas nécessaire de mettre en place une gestion des accès basée sur les rôles.
- Ajouter la possibilité pour un utilisateur de gérer un panier d'achat pouvant contenir des produits.
- Ajouter la possibilité pour un utilisateur de gérer une liste d'envie pouvant contenir des produits.

## Bonus

Vous pouvez ajouter des tests Postman ou Swagger pour valider votre API

## Livraison Back-end

L'API est développée en C# avec ASP.NET Core Web API 8.0.7 LTS, utilisant JWT Bearer Authentication pour l’authentification et SQL Server (en l’occurrence LocalDB) pour la base de données.

J’ai organisé l’architecture du projet en quatre parties :

- Domain : pour les entités métiers et les enums

- Infrastructure : pour la gestion de la base de données et des configurations

- Features : pour la logique métier et les services

- Shared : pour tout ce qui sera exposé, notamment les DTOs et les extensions

J’utilise AutoMapper pour le mapping des données avec la logique suivante : Entité ⇆ Model ⇆ DTO. Cela permet une meilleure autonomie dans la manipulation des données.

J'ai mis en place système d’authentification avec gestion des rôles, une pagination pour lister les produits, les dates en UTC pour laisser le front gérer le local.

Un seed est inclus dans le projet pour créer les utilisateurs de base, dont un compte admin.  
⚠️ La route `/account` permet également de créer un compte administrateur (**non recommandé en production**, mais laissé activé pour les tests).

### Comptes créés automatiquement démarrage

| Rôle  | Username | Firstname       | Email             | Mot de passe |
|-------|----------|----------------|-------------------|--------------|
| Admin | admin    | adminFirstname | admin@admin.com   | admin123     |
| User  | user     | userFirstname  | user@user.com     | user123      |

### Cela vous permet de :
- Tester rapidement avec le compte **admin** généré par le seed
- Créer d'autres comptes **admin** ou **user** si nécessaire lors des tests

J'ai également mis en place Swagger pour tester plus facilement : http://localhost:5020/swagger/index.html

###  Démarrage

Dans ```product-trial-master\back\altenshop\Api```

```bash
# Restaurer les dépendances
dotnet restore

# Appliquer les migrations
dotnet ef database update

# Lancer le projet
dotnet run
```