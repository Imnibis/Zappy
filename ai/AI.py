from Client import Client
from Map import Map
from Player import Player
from Socket import Socket

def loop(cli:Client, sock:Socket, map_info:Map) -> None:
    ply = Player()
    ply.update_pinv(sock)
    while (ply.status == True):
        if (ply.lvl == 1):
            pass
        elif (ply.lvl == 2):
            pass
        elif (ply.lvl == 3):
            pass
        elif (ply.lvl == 4):
            pass
        elif (ply.lvl == 5):
            pass
        elif (ply.lvl == 6):
            pass
        elif (ply.lvl == 7):
            pass
        ply.update_pinv(sock)