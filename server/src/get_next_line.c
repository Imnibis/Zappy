/*
** EPITECH PROJECT, 2019
** get_next_line.c
** File description:
** getnextline.c
*/

#include <stdlib.h>
#include <fcntl.h>
#include <unistd.h>

static int my_strclen(char *str)
{
    int i = 0;

    for (; str[i]; i++);
    return i;
}

static char *my_strncpy(char *res, char *str, int counter)
{
    int i = 0;

    for (; str[i] && i < counter; i++)
        res[i] = str[i];
    if (counter < i)
        res[i] = '\0';
    return res;
}

static char *add_line(char *line, int i, char *stock, int *n)
{
    char *tmp;
    int oldvalue;

    if (line == 0)
        oldvalue = 0;
    else
        oldvalue = my_strclen(line);
    tmp = malloc((oldvalue + i + 1) * sizeof(*tmp));
    if (line != 0)
        my_strncpy(tmp, line, oldvalue);
    else
        my_strncpy(tmp, "", oldvalue);
    tmp[oldvalue + i] = 0;
    my_strncpy(tmp + oldvalue, stock + *n, i);
    if (line)
        free(line);
    *n = *n + (i + 1);
    return tmp;
}

char *get_the_line(int fd, int rd)
{
    char stock[rd];
    static int current_arg = 0;
    static int n;
    char *line = NULL;

    for (int i = 0; 1; i++) {
        if (current_arg <= n) {
            n = 0;
            if (!(current_arg = read(fd, stock, rd)))
                return (line);
            if (current_arg == -1)
                return (NULL);
            i = 0;
        }
        if (stock[n + i] == '\n')
            return (line = add_line(line, i, stock, &n));
        if ((n + i) == (current_arg - 1))
            line = add_line(line, i + 1, stock, &n);
    }
}

char *get_next_line(int fd)
{
    int rd = 200;

    if (rd >= 1000)
        return NULL;
    else if (rd <= 0)
        return NULL;
    else
        return get_the_line(fd, rd);
}