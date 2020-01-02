CxList methods = Find_Methods();

//libpqxx
/*
pqxx::connection_base::esc (const char str[])
 	Escape string for use as SQL string literal on this connection.
pqxx::connection_base::esc (const char str[], size_t maxlen)
 	Escape string for use as SQL string literal on this connection.
pqxx::connection_base::esc (const std::string &str)
 	Escape string for use as SQL string literal on this connection.
pqxx::connection_base::esc_raw (const unsigned char str[], size_t len)
 	Escape binary string for use as SQL string literal on this connection.
pqxx::connection_base::quote_raw (const unsigned char str[], size_t len)
 	Escape and quote a string of binary data.
pqxx::connection_base::quote_name (const std::string &identifier)
 	Escape and quote an SQL identifier for use in a query.
*/
CxList libpqxxSanitizers = methods.FindByMemberAccess("connection.esc");
libpqxxSanitizers.Add(methods.FindByMemberAccess("connection.esc_raw"));
libpqxxSanitizers.Add(methods.FindByMemberAccess("connection.quote_raw"));
libpqxxSanitizers.Add(methods.FindByMemberAccess("connection.quote_name"));

/*
pqxx::transaction_base::esc (const char str[]) const
 	Escape string for use as SQL string literal in this transaction.
pqxx::transaction_base::esc (const char str[], size_t maxlen) const
 	Escape string for use as SQL string literal in this transaction.
pqxx::transaction_base::esc (const std::string &str) const
 	Escape string for use as SQL string literal in this transaction.
pqxx::transaction_base::esc_raw (const unsigned char str[], size_t len) const
 	Escape binary data for use as SQL string literal in this transaction.
pqxx::transaction_base::esc_raw (const std::string &) const
 	Escape binary data for use as SQL string literal in this transaction.
pqxx::transaction_base::quote (const T &t) const
 	Represent object as SQL string, including quoting & escaping.
pqxx::transaction_base::quote_raw (const unsigned char str[], size_t len) const
 	Binary-escape and quote a binarystring for use as an SQL constant.
pqxx::transaction_base::quote_raw (const std::string &str) const
pqxx::transaction_base::quote_name (const std::string &identifier) const
 	Escape an SQL identifier for use in a query.
*/
libpqxxSanitizers.Add(methods.FindByMemberAccess("work.esc"));
libpqxxSanitizers.Add(methods.FindByMemberAccess("work.esc_raw"));
libpqxxSanitizers.Add(methods.FindByMemberAccess("work.quote"));
libpqxxSanitizers.Add(methods.FindByMemberAccess("work.quote_raw"));
libpqxxSanitizers.Add(methods.FindByMemberAccess("work.quote_name"));

/*
pqxx::escape_binary (const std::string &bin)
 	Escape binary string for inclusion in SQL.
pqxx::escape_binary (const char bin[])
 	Escape binary string for inclusion in SQL.
pqxx::escape_binary (const char bin[], size_t len)
 	Escape binary string for inclusion in SQL.
pqxx::escape_binary (const unsigned char bin[])
 	Escape binary string for inclusion in SQL.
pqxx::escape_binary (const unsigned char bin[], size_t len)
 	Escape binary string for inclusion in SQL.
*/
libpqxxSanitizers.Add(methods.FindByShortName("escape_binary"));

//libpq
/*
PQescapeLiteral(PGconn *conn, const char *str, size_t length);
PQescapeIdentifier(PGconn *conn, const char *str, size_t length);
PQescapeStringConn(PGconn *conn, char *to, const char *from, size_t length, int *error);
PQescapeString (char *to, const char *from, size_t length);
PQescapeBytea(const unsigned char *from, size_t from_length, size_t *to_length);
PQescapeByteaConn(PGconn *conn, const unsigned char *from, size_t from_length, size_t *to_length);
*/
List<string> escapeMethods = new List<string>{
		"PQescapeBytea", "PQescapeIdentifier", "PQescapeLiteral", 
		"PQescapeString", "PQescapeStringConn", "PQescapeByteaConn"
		};
result.Add(methods.FindByShortNames(escapeMethods));
result.Add(libpqxxSanitizers);