-- Add allowed users to private decks
CREATE TABLE IF NOT EXISTS subscription_users_deck (
    user_id UUID NOT NULL,
    deck_id UUID NOT NULL,
    CONSTRAINT user_id_fk FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
    CONSTRAINT deck_id_fk FOREIGN KEY (deck_id) REFERENCES decks(id) ON DELETE CASCADE,
    CONSTRAINT subscription_users_deck_pk PRIMARY KEY (user_id, deck_id)
);