"""
This script runs the WebMap application using a development server.
"""

from os import environ
from WebMap import app

if __name__ == '__main__':
    '''
    HOST = environ.get('SERVER_HOST', 'localhost')
    try:
        PORT = int(environ.get('SERVER_PORT', '5555'))
    except ValueError:
        PORT = 5555
        '''
    app.debug = True
    app.run('localhost', 5000)
