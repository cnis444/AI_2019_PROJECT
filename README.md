# AI Project 2019

## Introduction

Ce projet à été réalisé dans le cadre de la formation de l'option IA de l'Enseirb-Matmeca. Il s'agit d'un projet semestriel dont le but est de développer un jeu vidéo. Ce projet à pour but d'utiliser plusieurs domaines de l'IA, tel que la génération procédurale ou la satisfaction de contraintes.  
Ce projet a été réaliser à l'aide de Unity 2019.2.10f1 et AbsCon19-06.

## Génération procédurale

### Monde
Le monde est généré de manière procédurale à l'aide d'un bruit de Perlin superposé sur plusieurs octaves. Les biomes sont définis selon les valeurs du bruit.

(Voir AI_2019_PROJECT/AI_Project/Assets/Scripts/Map/Noise pour le Perlin Noise)     
(Voir AI_2019_PROJECT/AI_Project/Assets/Scripts/Map/MeshGenerator pour la creation d'un chunk)     
(Voir AI_2019_PROJECT/AI_Project/Assets/Scripts/Map/CreateTerrain pour la creation des chunks avec batiment)     

### Bâtiments
Les bâtiments sont également généré de manière procédurale selon l'algorithme suivant :
1. Définir une zone correspondant à la taille maximale du bâtiment
2. Subdiviser cette zone en sous zones plus petites
3. Supprimer aléatoirement certaines de ces petites zones
4. Construire les murs extérieurs en suivant le contour des zones restantes
5. Recommencer depuis l'étape 3 pour ajouter un étage

(Voir AI_2019_PROJECT/AI_Project/Assets/Scripts/Building/SpacePartition pour le partitionage de l'espace)     
(Voir AI_2019_PROJECT/AI_Project/Assets/Scripts/Building/Building pour la generation des batiments)     


## Satisfaction de contraintes

Un système de contraintes à été mis en place afin de définir les différentes contraintes du monde tel que la position des bâtiments, des ennemis, les quêtes, etc. Ce système de contraintes est envoyé vers AbsCon pour être résolu. Une fois cela fait, la solution est récupérée et appliquée.

(Voir AI_2019_PROJECT/AI_Project/Assets/Scripts/Constraints/ConstraintObjects pour la class qui encapsule la list des contraintes)    
(Voir AI_2019_PROJECT/AI_Project/Assets/Scripts/Constraints/Constraints pour la list des contraintes)    
(Voir AI_2019_PROJECT/AI_Project/Assets/Scripts/Constraints/Solver pour la resolution des contraintes)    

## Système de quêtes

Un système de quêtes a également été mis en place permettant que créer des missions uniques d'une partie à l'autre. Chaque quête dispose de plusieurs objectifs et chaque objectif requière l'interaction avec plusieurs objets et/ou personnages.

(Voir AI_2019_PROJECT/AI_Project/Assets/Scripts/Quest/Quest pour le detail de la composition d'une quest)    
(Voir AI_2019_PROJECT/AI_Project/Assets/Scripts/Quest/QuestManager pour la gestion des quetes)    
(Voir AI_2019_PROJECT/AI_Project/Assets/Scripts/Quest/QuestUIMAnager pour la gestion de l'UI)     
(Voir QuestCollider QuestDestroyer  QuestInteract QuestTimer pour les trigger de quetes)    

## PNJs (personnages non joueurs)

Le comportement des PNJs est défini selon un Arbre de Décision. Grace à ces arbres, plusieurs comportements peuvent être codés permettant de créer plusieurs type d'ennemis, comme des sbires où des boss.

(Voir AI_2019_PROJECT/AI_Project/Assets/Scripts/NPC/NPC pour la classe mere)    
(Voir AI_2019_PROJECT/AI_Project/Assets/Scripts/NPC/FieldOfView/FieldOfView pour comment les NPC detect des objets)    
(Voir AI_2019_PROJECT/AI_Project/Assets/Scripts/NPC/AI/Guard/GuardAI pour l'IA du guard)   
(Voir les script avec Behavior dans leur nom dans AI_2019_PROJECT/AI_Project/Assets/Scripts/NPC/AI/Guard/ pour le details de chaque state du Behavior graphe du guard)    
(Voir AI_2019_PROJECT/AI_Project/Assets/Scripts/NPC/AI/Guard/GuardAI pour l'IA du guard)     
(Voir les script avec Behavior dans leur nom dans AI_2019_PROJECT/AI_Project/Assets/Scripts/NPC/AI/Guard/ pour le details de chaque state du Behavior graphe du guard)     


## Configuration

Nous avons ajouté un panneau de configuration affiché au lancement permettant à l'utilisateur de configurer les différents paramètres de génération du jeu. Grâce à cela, le joueur peut réellement rendre chaque partie unique.

(Voir AI_2019_PROJECT/AI_Project/Assets/Scripts/Menu/ButtonMenu pour la gestion de tous les menus de configuration, attention script un peu lourd a lire)     
(Voir AI_2019_PROJECT/AI_Project/Assets/Scripts/Menu/SetUp pour la jonction entre la configuration, les contraintes, les quetes)    

### Voir aussi

AI_2019_PROJECT/AI_Project/Assets/Scripts/Player/ pour le control du player    
Le dossier AI_2019_PROJECT/AI_Project/Assets/Animators/ qui contient les behavior graph sous forme de character controller    
AI_2019_PROJECT/AI_Project/Assets/Imports/NavMeshComponents/ contient la library de Unity pour les NavMesh    
Les parametres de configuration sont stocker dans le dossier : %userprofile%\AppData\Local\Packages\<productname>\LocalState   
