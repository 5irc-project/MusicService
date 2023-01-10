using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MusicService.Migrations
{
    /// <inheritdoc />
    public partial class CreationMusicServiceDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "t_e_genre_gen",
                schema: "public",
                columns: table => new
                {
                    genid = table.Column<int>(name: "gen_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    genname = table.Column<string>(name: "gen_name", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_genre_gen", x => x.genid);
                });

            migrationBuilder.CreateTable(
                name: "t_e_mood",
                schema: "public",
                columns: table => new
                {
                    moodid = table.Column<int>(name: "mood_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    moodname = table.Column<string>(name: "mood_name", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_mood", x => x.moodid);
                });

            migrationBuilder.CreateTable(
                name: "t_e_music_mus",
                schema: "public",
                columns: table => new
                {
                    musid = table.Column<int>(name: "mus_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    musartistname = table.Column<string>(name: "mus_artist_name", type: "text", nullable: true),
                    mustrackname = table.Column<string>(name: "mus_track_name", type: "text", nullable: true),
                    muspopularity = table.Column<float>(name: "mus_popularity", type: "real", nullable: true),
                    musacousticness = table.Column<float>(name: "mus_acousticness", type: "real", nullable: true),
                    musdanceability = table.Column<float>(name: "mus_danceability", type: "real", nullable: true),
                    musdurationms = table.Column<float>(name: "mus_duration_ms", type: "real", nullable: true),
                    muskey = table.Column<string>(name: "mus_key", type: "text", nullable: true),
                    mustempo = table.Column<float>(name: "mus_tempo", type: "real", nullable: true),
                    musenergy = table.Column<float>(name: "mus_energy", type: "real", nullable: true),
                    musinstrumentalness = table.Column<float>(name: "mus_instrumentalness", type: "real", nullable: true),
                    musliveness = table.Column<float>(name: "mus_liveness", type: "real", nullable: true),
                    musloudness = table.Column<float>(name: "mus_loudness", type: "real", nullable: true),
                    musspeechiness = table.Column<float>(name: "mus_speechiness", type: "real", nullable: true),
                    musvalence = table.Column<float>(name: "mus_valence", type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_music_mus", x => x.musid);
                });

            migrationBuilder.CreateTable(
                name: "t_e_playlist_plst",
                schema: "public",
                columns: table => new
                {
                    plstid = table.Column<int>(name: "plst_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    usrid = table.Column<int>(name: "usr_id", type: "integer", nullable: false),
                    moodid = table.Column<int>(name: "mood_id", type: "integer", nullable: false),
                    plstname = table.Column<string>(name: "plst_name", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_playlist_plst", x => x.plstid);
                    table.ForeignKey(
                        name: "FK_t_e_playlist_plst_t_e_mood_mood_id",
                        column: x => x.moodid,
                        principalSchema: "public",
                        principalTable: "t_e_mood",
                        principalColumn: "mood_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GenreMusic",
                schema: "public",
                columns: table => new
                {
                    GenresGenreId = table.Column<int>(type: "integer", nullable: false),
                    MusicsMusicId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenreMusic", x => new { x.GenresGenreId, x.MusicsMusicId });
                    table.ForeignKey(
                        name: "FK_GenreMusic_t_e_genre_gen_GenresGenreId",
                        column: x => x.GenresGenreId,
                        principalSchema: "public",
                        principalTable: "t_e_genre_gen",
                        principalColumn: "gen_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenreMusic_t_e_music_mus_MusicsMusicId",
                        column: x => x.MusicsMusicId,
                        principalSchema: "public",
                        principalTable: "t_e_music_mus",
                        principalColumn: "mus_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MusicPlaylist",
                schema: "public",
                columns: table => new
                {
                    MusicsMusicId = table.Column<int>(type: "integer", nullable: false),
                    PlaylistsPlaylistId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicPlaylist", x => new { x.MusicsMusicId, x.PlaylistsPlaylistId });
                    table.ForeignKey(
                        name: "FK_MusicPlaylist_t_e_music_mus_MusicsMusicId",
                        column: x => x.MusicsMusicId,
                        principalSchema: "public",
                        principalTable: "t_e_music_mus",
                        principalColumn: "mus_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MusicPlaylist_t_e_playlist_plst_PlaylistsPlaylistId",
                        column: x => x.PlaylistsPlaylistId,
                        principalSchema: "public",
                        principalTable: "t_e_playlist_plst",
                        principalColumn: "plst_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GenreMusic_MusicsMusicId",
                schema: "public",
                table: "GenreMusic",
                column: "MusicsMusicId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicPlaylist_PlaylistsPlaylistId",
                schema: "public",
                table: "MusicPlaylist",
                column: "PlaylistsPlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_playlist_plst_mood_id",
                schema: "public",
                table: "t_e_playlist_plst",
                column: "mood_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenreMusic",
                schema: "public");

            migrationBuilder.DropTable(
                name: "MusicPlaylist",
                schema: "public");

            migrationBuilder.DropTable(
                name: "t_e_genre_gen",
                schema: "public");

            migrationBuilder.DropTable(
                name: "t_e_music_mus",
                schema: "public");

            migrationBuilder.DropTable(
                name: "t_e_playlist_plst",
                schema: "public");

            migrationBuilder.DropTable(
                name: "t_e_mood",
                schema: "public");
        }
    }
}
