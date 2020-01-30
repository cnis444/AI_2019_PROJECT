# AI Project 2019

## Introduction

Ce projet à été réalisé dans le cadre de la formation de l'option IA de l'Enseirb-Matmeca. Il s'agit d'un projet semestriel dont le but est de développer un jeu vidéo. Ce projet à pour but d'utiliser plusieurs domaines de l'IA, tel que la génération procédurale ou la satisfaction de contraintes.  
Ce projet a été réaliser à l'aide de Unity 2019.2.10f1 et AbsCon19-06.

## Génération procédurale

### Monde
Le monde est généré de manière procédurale à l'aide d'un bruit de Perlin superposé sur plusieurs octaves. Les biomes sont définis selon les valeurs du bruit.

### Bâtiments
Les bâtiments sont également généré de manière procédurale selon l'algorithme suivant :
1. Définir une zone correspondant à la taille maximale du bâtiment
2. Subdiviser cette zone en sous zones plus petites
3. Supprimer aléatoirement certaines de ces petites zones
4. Construire les murs extérieurs en suivant le contour des zones restantes
5. Recommencer depuis l'étape 3 pour ajouter un étage

## Satisfaction de contraintes

Un système de contraintes à été mis en place afin de définir les différentes contraintes du monde tel que la position des bâtiments, des ennemis, les quêtes, etc. Ce système de contraintes est envoyé vers AbsCon pour être résolu. Une fois cela fait, la solution est récupérée et appliquée.

## Système de quêtes

Un système de quêtes a également été mis en place permettant que créer des missions uniques d'une partie à l'autre. Chaque quête dispose de plusieurs objectifs et chaque objectif requière l'interaction avec plusieurs objets et/ou personnages.

## PNJs (personnages non joueurs)

Le comportement des PNJs est défini selon un Arbre de Décision. Grace à ces arbres, plusieurs comportements peuvent être codés permettant de créer plusieurs type d'ennemis, comme des sbires où des boss.
