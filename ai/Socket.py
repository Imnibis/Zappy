#!/usr/bin/env python3
##
## EPITECH PROJECT, 2020
## B-YEP-410-LYN-4-1-zappy-arthur1.perrot [WSL: Debian]
## File description:
## main.py
##

import socket
from sys import stderr
from sys import exit as xit

class Socket():
    def __init__(self) -> None:
        self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    def connect(self, host:str, port:int) -> None:
        try:
            self.sock.connect((host, port))
        except ConnectionRefusedError:
            stderr.write("ConnectionRefusedError: can't connect to server\n")
            xit(84)

    def send(self, msg:str) -> None:
        self.sock.send(msg.encode())

    def receive(self) -> str:
        msg = str('')
        while (len(msg) < 1000):
            chunk = self.sock.recv(1000 - len(msg))
            if chunk.find(10) != -1:
                msg += str(chunk, "utf-8")
                return msg
        msg += str(chunk, "utf-8")
        return msg