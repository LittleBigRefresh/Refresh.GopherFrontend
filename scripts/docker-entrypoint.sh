#!/bin/sh

# Docker volume mapping messes with folder ownership
# so it must be changed again during the entrypoint
chown -R refresh:refresh /refresh/data

cd /refresh/data

exec su-exec refresh /refresh/app/Refresh.GopherFrontend

exit $? # Expose error code