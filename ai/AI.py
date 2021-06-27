from sys import stderr
from Client import Client
from Map import Map
from Player import Player
from Socket import Socket
from subprocess import run

def get_tiles_content(sock:Socket, ply:Player) -> list:
    sock.send("Look\n")
    tiles = sock.receive()
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
        for j in range(len(tiles[i])):
            if tiles[i][j] == stone:
                return i + 1
    return -1

def forkAI(sock:Socket, cli:Client, ply:Player) -> None:
    if (cli.can_connect > 0):
        sock.send("Fork\n")
        if sock.receive() == "ok\n":
            cli.can_connect -= 1
            run(["python3", "main.py", "-p", str(cli.port), "-n", cli.name, "-h", cli.machine])

def loop(cli:Client, sock:Socket, map_info:Map, ply:Player) -> None:
    ply.update_pinv(sock)
    while ply.status == True:
        tiles = get_tiles_content(sock, ply)
        if (ply.pin[0] < 15):
            food = search_food(tiles)
            if food != -1:
                ply.go_to(sock, food)
                ply.take_obj(sock, "food")
                while (sock.receive() != "ko\n") and (ply.pin[0] < 15):
                    ply.update_pinv(sock)
                    ply.take_obj(sock, "food")
        if (ply.check_requirement() == True):
            ply.start_incantation(sock)
        else:
            stone = ply.needed_stone()
            tile = search_stone(tiles, stone)
            if tile != -1:
                ply.go_to(sock, tile)
                ply.take_obj(sock, stone)
                while (sock.receive() != "ko\n"):
                    ply.update_pinv(sock)
                    ply.take_obj(sock, stone)
            else:
                sock.send("Right\n")
                sock.receive()
                sock.send("Forward\n")
                sock.receive()
        forkAI(sock, cli, ply)