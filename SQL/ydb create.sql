CREATE TABLE games
(
    game_id Utf8,
    game_date Datetime,
    title Utf8,
    PRIMARY KEY (game_id)
);

CREATE TABLE game_participants
(
    game_id Utf8,    
    participant_name Utf8,
    state Utf8,
    participant_count Int32,
    PRIMARY KEY (game_id, participant_name)
);