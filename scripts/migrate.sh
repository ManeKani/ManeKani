#!/bin/sh

PROJECT_ROOT=$(dirname $(dirname $(realpath $0)))
DATABASE_URL=${DATABASE_URL:-"postgres://manekani:secret@localhost:5433/manekani-test"}
MIGRATIONS_DIR=${MIGRATIONS_DIR:-"$PROJECT_ROOT/Migrations"}

# Run or revert migrations
case $1 in
  run)
    echo "Running migrations..."
    sqlx migrate run --source $MIGRATIONS_DIR
    ;;
  revert)
    echo "Reverting migrations..."
    sqlx migrate revert --source $MIGRATIONS_DIR 
    ;;
  revert-all)
    echo "Reverting all migrations..."
    sqlx migrate revert --target-version 0 --source $MIGRATIONS_DIR
    ;;
  *)
    echo "Usage: $0 {run|revert}"
    exit 1
    ;;
esac