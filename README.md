# MusicService
Microservice for our final year project to handle musics

En gros faut que dans TrackService j'add les genres à ladite musique dans la table de jointure via le repo, pareil pour Playlist et track jpense.


(Code juste pr mettre les tracks / genres direct en post sur les endpoints que j'ai virer mais on sait jamais)

```c#
if (genreIds.All(id => _context.Genres.AsNoTracking().FirstOrDefault(g => g.GenreId == id) != null) == true){
    Track t = _mapper.Map<Track>(tDTO);
    t.TrackGenres = new List<TrackGenre>();
    #pragma warning disable CS8601  
    genreIds.ForEach(id => {
        t.TrackGenres.Add(
            new TrackGenre {
                Track = t,
                Genre = _context.Genres.Find(id)
            }
        );
    });
    #pragma warning restore CS8601  
    _context.Tracks.Add(t);
    await _context.SaveChangesAsync();
}else{
    throw new GenreNotFoundException("At least one of the given genre does not exist");
}
```

TODO :

Check that stuff doesn't already exist when adding genres or tracks to tracks or playlists
Also return a notfound error or smthing when trying to add non existant genres / tracks to tracks / playlists
Then do tests
Then isoki guess

En gros le pb c'était que quand j'ajoute une musique si je veux lui associer un genre faut que je le get via le context comme ça la bdd le track et ne va pas tenter de le créer une nouvelle fois.
