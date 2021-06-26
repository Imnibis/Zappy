from typing import List
from Client import Client
from Map import Map
from Player import Player
from Socket import Socket
from sys import stderr

def get_tiles_content(sock:Socket) -> list:
    sock.send("Look\n")
    tiles = sock.receive()
    tiles = tiles[2:-2]
    tiles = tiles.split(', ')
    for i in range(len(tiles)):
        tiles[i] = tiles[i].split(' ')
    return tiles

def search_food(tiles:list) -> int:
    i = 0
    while tiles[i]:
        for j in range(len(tiles[i])):
            if tiles[i][j] == "food":
                return i
        i += 1
    return -1

def search_stone(tiles:list, stone:str) -> int:
    i = 0
    while tiles[i]:
        for j in range(len(tiles[i])):
            if tiles[i][j] == stone:
                return i
        i += 1
    return -1

def loop(cli:Client, sock:Socket, map_info:Map) -> None:
    ply = Player()