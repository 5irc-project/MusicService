# MusicService
Microservice for our final year project to handle musics

TOFIX :

Pour les add track to playlist et add genre to tracks faudrait return si jamais on essaye d'add un genre qui existe déjà ?
En fait jpense faut faire un entitystate modified ou qq chose comme ça jsais pas
Aussi ptet virer les musiques ac artistaName = empty_field ?
Faire les tests

Possible pb avec les tests => le sequenceEqual prend en compte l'ordre de retour (je crois), donc c potentiellement dla merde

Attention sur les tests, faut faire gaffe aux AreEqual ça use la reference des fois (voir PostGenre les tests pr un truc ok)

TODO :

Add constructor for Track & other DTOs for easier testing ? (see every test)
Add mapping from Tracks to TrackWithGenresDTO for Genres ? (see test TrackController, PostTrackWithGenres_ReturnsOk);
Add mapping from TrackWithGenresDTO to TrackDTO ? (see GetTrack_ReturnsOk)
Can't test cascade deletes, not supported InMemory (or mock), and also apprently can't test foreign keys ?
Return message that states what happened when adding / removing genres ? (yes)
Make Name value for Genre / Kind nullable maybe, it's annoying otherwise (dunno)
Add Check for user with playlist
Add genres when getting playlist


TOADD :

Tests pour post (maintenant je renvoie l'id donc faut voir si ça marche vraiment)

CHIANT :

RAbbit mq mets 15 ans ?
Pas de type générique pr QueueMessage (naze)
La d

Ajouter une méthode pr créer une playlist favoris pr un connard (peut y en avoir qu'une)