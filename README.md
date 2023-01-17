# MusicService
Microservice for our final year project to handle musics

TESTING:
AddFavorite already exists => Marche par parce que ça apelle une autre méthode jpense qui utilise pas la même DB...
Update les tests playlist pr vérif qu'il y a bien les genres ?

TOFIX :
Aussi ptet virer les musiques ac artistaName = empty_field ?
TrackGenre comme PlaylistTrack (au niveau du AddGenreToTrack)
Le broker c'est naze de mettre dans les constructeurs !

TODO :
Return message that states what happened when adding / removing genres ? (yes)
Faut trouver comment lancer la migration au lancement et tt ça
Comment test le truc ac Authorize ?
Test les deux nouvelles methodes
Faire un discovery dev