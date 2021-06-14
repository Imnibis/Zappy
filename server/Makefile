##
## EPITECH PROJECT, 2019
## makefile
## File description:
## my_sokoban compile
##

SRC		=	main.c	\
			config.c	\
			utils.c

OBJ		=	$(SRC:.c=.o)

NAME		=	zappy_server

CFLAGS		+=	-W -Wall -Wextra -pedantic

all:		$(NAME)

$(NAME):	$(OBJ)
		gcc -o $(NAME) $(OBJ) $(CFLAGS)

clean:
		rm -f $(OBJ)

fclean:		clean
			rm -rf $(NAME)

re:		fclean all
