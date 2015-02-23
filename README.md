# MangoPayTuto

Issue de l'article du blog http://blog.reactor.fr:
http://blog.reactor.fr/post/mangopay-une-api-de-gestion-de-paiement-sur-internet

MangoPay est une solution de gestion de paiement sur internet, l'équivalent de son homologue US Paypal mais, cocorico, cette solution est Française. Enfin presque, car la société est basée à Luxembourg... En même temps pour une "pseudo" banque, cela reste logique. En effet MangoPay est une banque qui n'en est pas une. C'est plutôt un intermédiaire qui facilite les paiements et s'assure (avec les vraies banques) que les paiements soient effectués. En anglais on appelle cela un "Escrow" (sans déconner !). Un opérateur tiers. 

Donc un "Escrow" ne se traduit pas par "escroc" (escrow vient du vieux français escroue, qui signifiait "bout de parchemin") mais plutôt par "dépôt", "compte bloqué". Il faut le voir comme un intermédiaire entre les clients de votre site e-commerce et les banques de ces clients. MangoPay permet à de jeune startup de créer un porte-monnaie virtuel en marque blanche. Bien évidement une commission est prélevée (1.8% + 0.18€ --> détail)

MangoPay se veut être une plateforme de paiement en ligne pour les Marketplaces, les plateformes collaboratives et les sites de crowdfunding. Bien évidement les workflows ne sont pas les mêmes suivant ces cas. En ce qui me concerne, je vais m'interresser aux modèles des sites e-commerces. Ma problématique est de mettre en relation client et acheteur, tout en garantissant que l'acte d'achat se déroule proprement. Ceci implique de gérer le suivis des livraisons et des retours, voir même les échanges physiques réel.

Pour cela je vais utiliser l'API de MangoPay. Comme je vais devoir manipuler de l'argent, des cartes de crédits, des porte-monnaies, MangoPay met en place deux environnements, la production classique et le mode sandbox. C'est sur ce dernier que je vais tester mon application. En plus d'être "fictif" il permet de tester des cartes bancaires factice et donc d'ajouter de l'argent sur les portes-monnaies virtuels de mes utilisateurs, en ne déboursant pas un rond !

Mises en place de l'API - Inscription

https://docs.mangopay.com

Vous devez créer deux comptes, un pour la prod (pour plus tard, quand vous serez prêt) et un pour la sandbox. Je vous conseil de faire les deux tout de suite, comme ca c'est fait, et vous allez conserver ses informations dans votre coffre-fort. 

Rendez vous sur cette page https://docs.mangopay.com/api-references/start-in-production/ pour l'inscription en prod, et sur cette page https://docs.mangopay.com/api-references/sandbox-credentials/ pour l'inscription en mode sandbox.

On vous demandera de créer un clientId, de donner le nom de votre société et un email (choisir aussi la monnaie Euro). L'important c'est d'avoir un email valide (pro de préférence) et un clientId que vous allez utiliser par la suite dans l'API. Vous allez ensuite valider votre inscription et recevoir un mot de passe (passphrase). NOTEZ ces informations et conservez les !

Ça y est, vous avez créé vos accès à l'API, maintenant passons à la doc.

Les bases de l'API :

Bien sur il faut se référer avant tout à la doc online. Plutôt bien faite, du moins il y a bcp d'info, mais pas forcément affiché dans le bon ordre selon moi. Ce n'est que mon avis, mais je vous propose de regarder ces pages dans cet ordre (cas e-commerce) :

https://docs.mangopay.com/marketplace-workflow/ (workflow pour les sites e-commerce)
https://docs.mangopay.com/api-references/ (vous donne l'ensemble des requêtes possible)
https://docs.mangopay.com/mangopay-dashboard/ (accès au dashboard)

Et arrêtez-vous la pour l'instant. Créez vous un accès sur le dashboard Sandbox et familiarisez vous avec quelques minutes. Il est vide à ce stade donc pas grand chose à voir. 

L'important ici est de comprendre à quoi sert MangoPay et ce que vous voulez en faire. Ou trouvez l'information, les ressources et savoir utiliser les outils disponibles (dashboard). 

Le code C#

L'Api fonctionne avec l'architecture REST. Les avantages : simple, sans état : indépendance client-serveur, pas de session, représentation compréhensible sous forme d'URI. Les inconvénients : le client doit conserver certaines données, augmentation du nombre de requêtes. 

Une des pages de la doc conseille de commencer par créer un utilisateur (user), puis de lui associé un porte-monnaie (wallet). C'est ce que je vais vous montrer, mais en créant plutôt deux utilisateurs : John et Julie. 

Tout d'abord, vous devez télécharger le SDK de votre langage de programmation préféré. Le SDK MangoPay est disponible sur GitHub en PHP, Python, Ruby, Java, .Net et JavaScript. Je vais continuer l'article en m'appuyant sur le SDK .net, mais sachez les noms des méthodes sont les mêmes dans toutes les versions (attention, j'ai pas tout vérifié), et parfois avec les mêmes fautes d'orthographe ! (ceci ma permit d'en conclure qu'effectivement le code est fortement similaire)  

https://github.com/MangoPay

Note: j'ai commencé par utilisé la version compilé du package SDK .net dans une solution WebForm Asp.net. J'ai eu des soucis lors de l'execution sur les libs Common.Logging.Core.dll et Common.Logging.dll. J'ai pas compris pourquoi alors j'ai téléchargé le code source de MangoPay.SDK et j'ai ajouté créé un projet web directement dans leur solution. Ça compile et ca s'exécute correctement. Mais mieux, dans la solution fournit par le SDK on a un projet de Test, c'est une mine d'or pour comprendre comment fonctionne l'API. Je vous conseille fortement d'aller y faire un tour. J'ai autant appris avec les tests qu'avec la doc sur internet. 

Il faut pour cela instancier l'API et lui passer les paramètres suivant :

MangoPayApi api = new MangoPayApi ();
api.Config.ClientId = "myClientId" ;
api.Config.ClientPassword = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" ;

L'objet 'api' dispose d'une autre propriété importante : api.Config.BaseUrl. Par défaut c'est l'url de la SandBox. Donc si vous ne la spécifiez pas, vous serez par défaut sur le serveur de test. 

Pour créer un utilisateur, voici les quelques lignes de code :

UserNaturalPostDTO user = new UserNaturalPostDTO ("john.doe@sample.org" , "John" , "Doe" , new DateTime (1975, 12, 21, 0, 0, 0), CountryIso.FR, CountryIso .FR);
user.Occupation = "programmer" ;
user.IncomeRange = 3;
user.Address = "Some Address" ;

UserNaturalDTO john = api.Users.Create(user);

Remarque : MangoPay distingue deux types d'utlisateur, les UserNatural et les UserLegal. Pour faire simple, UserNatural est un utilisateur lambda, le client de base et UserLegal une personne morale, représentant une société. Ceci m'amène à vous parler du KYC Concept de MangoPay. KYC signifie "Know Your Customer", "Connais ton client". Bon tout ça découle certainement de directive Européenne et est en lien étroit avec la lutte contre la fraude, le blanchiment d'argent et le financement du terrorisme (rien contre l'évasion fiscale ? étonnant...) Du coup MangoPay distingue ces deux types d'utilisateur pour leur appliquer un ensemble de règles, des informations supplémentaire pour le UserLegal et des montants de transaction plafonnés (voir sur leur site web pour plus de détail). 

Notez que pour un objet standard, le SDK MangoPay attribut le suffixe DTO (Data Transfert Object). Pour un objet en création, ce sera PostDTO. Pour un update PutDTO et pour la lecture ca aurait pu etre GetDTO mais c'est juste DTO (objet standard). exemple WalletDTO, WalletPutDTO, WalletPostDTO. Et pour le delete ? Pas encore vu ce cas...

Notez aussi que la fonction Create retourne un UserNaturalDTO qui contient l'id du nouvel utilisateur crée. Il faudra impérativement stocker cet Id dans votre programme. (on peut le retrouver manuellement sur le DashBoard)

Revenons à nos utilisateurs. Créons de la même manière l'utilisateur Julie (je vous laisse deviner le code). 

Normalement vous verrez apparaitre sur votre Dashboard les deux nouveaux utilisateurs :



Ensuite il faut créer un porte-monnaie pour chacun. Cet objet s'appelle Wallet. (_sandbox_John_UserId est une variable globale remplie à la main pour l'instant)

List<string> owners = new List< string>();
owners.Add(_sandbox_John_UserId);
WalletPostDTO wallet = new WalletPostDTO (owners, "Wallet de John" , CurrencyIso.EUR);
WalletDTO johnWallet = api.Wallets.Create(wallet);

Donc c'est aussi très simple, on créer un wallet qui appartient (owners) à John. (a priori il est possible d'affecter un wallet à plusieurs détenteur.) On fait de même avec Julie. 

Workflow standard : John paie Julie pour l'achat d'un livre sur le site e-commerce :

Maintenant que l'on a deux utilisateurs qui ont chacun un portes-monnaies, on va affecter une somme d'argent sur chacun de ces portes-monnaies. Nous allons simuler (on est en mode sandbox, c'est pour du faux !) le transfert d'argent avec une carte de crédit comme si John "achetait" sur internet. Dans notre workflow, John achète un livre à Julie sur notre site e-commerce. Donc John doit mettre de l'argent sur son porte-monnaie, puis doit faire un transfert vers le porte-monnaie de Julie, qui elle devra à nouveau transférer l'argent sur son compte bancaire. 

Tout cela peut être totalement transparent pour les utilisateurs. C'est à dire qu'il ne s'aperçoivent même pas que leur argent transit via MangoPay. Tout ce fait en "backoffice". 

Notre site e-commerce (ou plutôt MangoPay) devra momentanément "garder" l'argent pour des questions de remboursement éventuel (litige, annulation, produit non conforme...). Une fois la vente effectuée (livraison ou main propre) nous pouvons finaliser la transaction en versant à Julie son argent. La commission du site e-commerce interviendrait à ce moment la.(https://docs.mangopay.com/marketplace-workflow/) La commission du site MangoPay interviendrait au moment ou John met de l'argent sur son porte-monnaie (Payin), ou durant un transfert d'argent (transfert), ou sur un transfert vers sa banque (Payout)  (https://docs.mangopay.com/fees-rules/). 

Dans tous les cas, les commissions de MangoPay sont prélevées sur le compte du détenteur (un wallet géré uniquement par MangoPay) et pas sur les clients. 

Exemple :
1) John paie 100€
2) John reçoit 100€ dans son wallet
2) Ces 100 € sont transféré dans le wallet de Julie.
3) Julie a donc 100€ sur son wallet et décide de récupérer cet argent sur son compte bancaire.
4) Julie reçoit 90€ (100€ - 10% de la commission du site e-commerce)

Ces 10€ de commission partent dans un wallet de MangoPay (on a pas accès) et restent pendant 1 mois. 
A la fin du mois, MangoPay prend sa commission de 1.8% (de la somme initial du produit, donc sur 100€) + 0.18€ (fixe). 
Soit 1.98€. Le restant (8.02€) nous est redistribué. 


Premier transfert d'argent

Pour mettre 100€ via carte bancaire sur le wallet de John, nous allons utiliser une fause carte bancaire donné par MangoPay : https://docs.mangopay.com/api-references/test-payment/

    N°: 4970100000000154
    Expiry: Any date in the future (ex : 1218)
    CSC: 123

J'ai essayé de réduire le code au minimum pour aboutir à ces 4 lignes :

UserNaturalDTO userJohn = Samples .GetJohn(api);
WalletDTO johnWallet = Samples.GetJohnWallet(api);
ReactorCreditCard fakeVisa = ReactorCreditCard .FakeVisa1();
Ninja.AddMoneyOnUserWalletByCreditCard(api, userJohn, johnWallet, fakeVisa, 100);

Pour cela j'ai créé 3 classes (dans le projet webtest, dossier helper)

- ReactorCreditCard.cs : c'est une classe rapide pour simuler une fausse carte de credit, des cartes périmées, des fausses cartes... bref ca dépanne. 
- Samples.cs : c'est la classe qui va généré des "échantillons", ici John et Julie avec leur wallet. 
- Ninja.cs : c'est une classe du helper qui va m'aider dans mes transaction avec MangoPay, ici j'y ai mis tout ce qui concerne l'ajout d'argent via carte bancaire (Ninja.AddMoneyOnUserWalletByCreditCard)

Ceci n'étant ici qu'a but éducatif et ne consiste pas en un code "à appliquer", vous êtes libre de prendre ce qu'il vous faut et de faire à votre sauce. 

Comment ça marche ?

Il faut bien suivre ce process : https://docs.mangopay.com/api-references/card-registration/. 

- On fait une demande d'enregistrement de carte sur MangoPay. 
- On recoit une url dans l'objet de retour (serveur de token)
- On requête le serveur de token pour obtenir un jeton. (en faite c'est ici que la carte CB est réellement vérifiée)
- Ce jeton nous permet alors d'enregister la carte sur MangoPay, ce qui nous permet alors d'avoir un objet carte MangoPay.
- à partir de ces deux objets (CardRegistrationDTO, CardDTO) on peut commencer le transfert.

Attention : la validité d'une CardRegistrationDTO avant un paiement ou une pre-autorisation est de 30 min. Une fois qu'une de ces opérations est effectuées, la validité de la carte (Validity) change en "VALID" et expirera qu'à sa date d'expiration. 

Voila, mission accomplie ! Vous pouvez aller voir sur le dashboard vos sous sous virtuels. 

J'espère que ce tuto vous a plu. N'hésitez pas à me laisser vos commentaires ! 



