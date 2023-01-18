# MusicService
Microservice for our final year project to handle musics

TESTING:
AddFavorite already exists => Marche par parce que ça apelle une autre méthode jpense qui utilise pas la même DB...
Update les tests playlist pr vérif qu'il y a bien les genres ?

TOFIX :
Aussi ptet virer les musiques ac artistaName = empty_field ?
TrackGenre comme PlaylistTrack (au niveau du AddGenreToTrack)

TODO :
Comment test le truc ac Authorize ?
Custom rabbit mq queue name and config
?
Search by artist name & artist

setup queueName & host rest