from Client import Client
from Map import Map
from Player import Player
from Socket import Socket

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

def loop(cli:Client, sock:Socket, map_info:Map, ply:Player) -> None:
    while(True):
        ply.broadcast_message(sock, str("Elevation 4"))
        print(sock.receive())
        parseMessage(sock.receive())

def parseMessage(message:str) -> list:
    parsedList = list()
    incr = 0

    while (message[incr - 1] != " "):
        incr += 1
    parsedList.append(message[incr])
    incr += 1
    while (message[incr - 1] != " "):
        incr += 1
    parsedList.append(message[incr:])
    print(parsedList[0])
    return parsedList