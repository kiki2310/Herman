#include <iostream>
#include <string>
#include <regex>

int main() {
	std::regex word_whattsapp("whatsapp", std::regex_constants::icase);

	std::string phrases[] = {
		"Hablamos por WhatsApp?",
		"Mi aplicacion favorita es Whatsapp.",
		"Estaba whatsappeando con mis amigos.",
		"Esto es una palabra clave: WHATSAPP.",
		"No hay menciones de la app aqui.",
	};
	std::cout << "Deteccion de la palabra 'whatsapp' en diferentes frases:\n\n";
	for (const std::string& text : phrases) {
		bool found = std::regex_search(text, word_whattsapp);
		std::cout << "Frase: \"" << text << "\"\n";
		std::cout << "Contiene 'whatsapp': " << (found ? "Si" : "No") << "\n\n";
		std::cout << "-----------------------------------\n";
	}
	return 0;
}