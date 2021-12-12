Hello,
Thank you for giving me the task. I thought it would be boring, but it was fun.It is more or less standard .net 5 api project. 

PokemonController - uses PokemonService for all requests.

PokemonService - responsible to access the pokemon API (via HttpService, and PokemonParser) to get pokemon data. 
It uses TranslationService to translate description if needed.

HttpService - a way to decouple http requestsPokemonParser - a strategy to parse pokemon API response.

PokemonParser - parse the pokemon API response.

TranslationService - translate description using the translate API. It uses the HttpService, and TranslationParser.

TranslationParser - parse the translation service API.

I have not added logging to the app, which is a good idea for production. 

Further refactoring could be done with the code (like combining Parsers into a common interface, 
and extracting yoda/shakespeare logic into a service), but better to stop somewhere.  

Small list of suggested pokemons to try:
bulbasaur (not in cave, and not legendary - shakespeare translation)
zubat - (in cave, but not legendary - yoda translation)
articuno - (not in cave, but legendary - yoda translation)
regirock - (both legendary and in cave - yoda translation)

Many thanks and have a good health,
Yuriy
