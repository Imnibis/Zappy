from Client import Client
from Map import Map
from Player import Player
from Socket import Socket
from os import fork

def get_tiles_content(sock:Socket, ply:Player) -> list:
    sock.send("Look\n")
    tiles = sock.receive(ply)
    tiles = tiles[2:-3]
    tiles = tiles.split(', ')
    for i in range(len(tiles)):
        tiles[i] = tiles[i].split(' ')
    return tiles

def search_food(tiles:list) -> int:
    for i in range(len(tiles)):
        for j in range(len(tiles[i])):
            if tiles[i][j] == "food":
                return i + 1
    return -1

def search_stone(tiles:list, stone:str) -> int:
    for i in range(len(tiles)):
        print(i)
        for j in range(len(tiles[i])):
            if tiles[i][j] == stone:
                return i + 1
    return -1

def loop(cli:Client, sock:Socket, map_info:Map, ply:Player) -> None:
    while ply.status == True:
        print(ply.status)
        sock.receive(ply)

def forkAI(sock:Socket, ply:Player):
    sock.send("Connect_nbr\n")
    if (sock.receive(ply) != "0\n"):
        sock.send("Fork\n")
        child = fork()
        if (child == 0):
            pass
