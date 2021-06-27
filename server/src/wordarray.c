/*
** EPITECH PROJECT, 2021
** NTP
** File description:
** wordarray
*/

#include <string.h>
#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <unistd.h>

int pos;

int nb_word(char const *str, char f)
{
    int word = 1;
    bool in_word = false;

    if (strlen(str) == 0)
        return 0;
    for (int i = 0; str[i]; i++) {
        if (str[i] == 34 && in_word == false) {
            in_word = true;
            i++;
        }
        for (; in_word == true && str[i]; i++) {
            if (str[i] == 34)
                in_word = false;
        }
        if (str[i] == f)
            word++;
    }
    return word;
}

int len_word(char const *str, char f)
{
    bool quote = false;
    int len = 0;

    for (; str[pos] && (str[pos] != f ||
    (str[pos] == f && quote == true)); pos++, len++) {
        if (str[pos] == 34) {
            if (quote == true)
                quote = false;
            else if (quote == false)
                quote = true;
        }
    }
    pos++;
    return len;
}

char *get_chrs(const char *str, int start, int end)
{
    char *tmp = malloc(sizeof(char) * (end - start) + 1);
    int j = 0;

    for (int i = start; i != end; i++) {
        if (str[i] != 34) {
            tmp[j] = str[i];
            j++;
        }
    }
    tmp[j] = '\0';
    return tmp;
}

char **str_warray(char const *str, char f)
{
    char **array = NULL;
    int words = nb_word(str, f);
    int len;
    int position = 0;

    if (strlen(str) == 0)
        return NULL;
    pos = 0;
    array = malloc(sizeof(char *) * (words + 1));
    for (int i = 0, j = 0; i < words; i++, j++) {
        len = len_word(str, f);
        array[i] = get_chrs(str, position, position + len);
        position += len + 1;
    }
    if (array[words - 1][strlen(array[words - 1]) - 1] == '\n')
        array[words - 1][strlen(array[words - 1]) - 1] = '\0';
    array[words] = NULL;
    return array;
}