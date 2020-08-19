--
-- PostgreSQL database dump
--

-- Dumped from database version 9.6.5
-- Dumped by pg_dump version 9.6.0

-- Started on 2020-08-19 11:26:44

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 3 (class 2615 OID 16555)
-- Name: auth; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA auth;


ALTER SCHEMA auth OWNER TO postgres;

--
-- TOC entry 2337 (class 0 OID 0)
-- Dependencies: 3
-- Name: SCHEMA auth; Type: COMMENT; Schema: -; Owner: postgres
--

COMMENT ON SCHEMA auth IS 'standard public schema';


--
-- TOC entry 5 (class 2615 OID 16869)
-- Name: core; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA core;


ALTER SCHEMA core OWNER TO postgres;

--
-- TOC entry 6 (class 2615 OID 16675)
-- Name: dating; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA dating;


ALTER SCHEMA dating OWNER TO postgres;

--
-- TOC entry 10 (class 2615 OID 16809)
-- Name: geo; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA geo;


ALTER SCHEMA geo OWNER TO postgres;

--
-- TOC entry 7 (class 2615 OID 16674)
-- Name: register; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA register;


ALTER SCHEMA register OWNER TO postgres;

--
-- TOC entry 1 (class 3079 OID 12387)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2338 (class 0 OID 0)
-- Dependencies: 1
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = auth, pg_catalog;

--
-- TOC entry 221 (class 1255 OID 16560)
-- Name: claim__create(character varying); Type: FUNCTION; Schema: auth; Owner: postgres
--

CREATE FUNCTION claim__create(p_claim_code character varying) RETURNS integer
    LANGUAGE plpgsql
    AS $$
	DECLARE createdClaimId INTEGER;
BEGIN
	
	INSERT INTO auth.claim(claim_code)
    VALUES (p_claim_code)
    RETURNING claim_id INTO createdClaimId;

    RETURN createdClaimId;
END;
$$;


ALTER FUNCTION auth.claim__create(p_claim_code character varying) OWNER TO postgres;

--
-- TOC entry 218 (class 1255 OID 16556)
-- Name: group__create(text); Type: FUNCTION; Schema: auth; Owner: postgres
--

CREATE FUNCTION group__create(group_name text) RETURNS integer
    LANGUAGE plpgsql
    AS $$
	DECLARE createdId INTEGER;
BEGIN
	
	INSERT INTO auth.group(group_name)
    VALUES (group_name)
    RETURNING group_id INTO createdId;

    RETURN createdId;
END;
$$;


ALTER FUNCTION auth.group__create(group_name text) OWNER TO postgres;

--
-- TOC entry 219 (class 1255 OID 16557)
-- Name: group_user__add(integer, integer); Type: FUNCTION; Schema: auth; Owner: postgres
--

CREATE FUNCTION group_user__add(group_id integer, user_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
	--DECLARE createdId INTEGER;
BEGIN
	
	INSERT INTO auth.group_user(group_id, user_id)
    VALUES (group_id, user_id);

END;
$$;


ALTER FUNCTION auth.group_user__add(group_id integer, user_id integer) OWNER TO postgres;

--
-- TOC entry 224 (class 1255 OID 16931)
-- Name: user__create(character varying, bit, character varying, character varying, integer); Type: FUNCTION; Schema: auth; Owner: postgres
--

CREATE FUNCTION user__create(email character varying, email_confirmed bit, pass_hash character varying, salt character varying, locale_id integer) RETURNS integer
    LANGUAGE plpgsql
    AS $$
	DECLARE createdUserId INTEGER;
BEGIN
	
	INSERT INTO auth.user(email, email_confirmed, pass_hash, salt, locale_id)
    VALUES (email, email_confirmed, pass_Hash, salt, locale_id)
    RETURNING user_id INTO createdUserId;

    RETURN createdUserId;
END;
$$;


ALTER FUNCTION auth.user__create(email character varying, email_confirmed bit, pass_hash character varying, salt character varying, locale_id integer) OWNER TO postgres;

--
-- TOC entry 245 (class 1255 OID 17011)
-- Name: user__get_all(); Type: FUNCTION; Schema: auth; Owner: postgres
--

CREATE FUNCTION user__get_all() RETURNS TABLE(user_id integer, email character varying, login character varying, pass_hash character varying, pass_salt character varying, date_created timestamp without time zone, date_last_login timestamp without time zone, locale_id integer, chosen_place_id integer)
    LANGUAGE plpgsql
    AS $$
DECLARE cur_1 refcursor;
BEGIN

	RETURN QUERY
    SELECT 
		u.user_id,
        u.email,
        u.login,
        u.pass_hash,
        u.pass_salt,
        u.date_created,
        u.date_last_login,
        u.locale_id,
  		u.chosen_place_id
    FROM 
      auth."user" u;
    
END;
$$;


ALTER FUNCTION auth.user__get_all() OWNER TO postgres;

--
-- TOC entry 241 (class 1255 OID 16999)
-- Name: user__get_all_ids(); Type: FUNCTION; Schema: auth; Owner: postgres
--

CREATE FUNCTION user__get_all_ids() RETURNS TABLE(user_id integer)
    LANGUAGE plpgsql
    AS $$
DECLARE cur_1 refcursor;
BEGIN

	RETURN QUERY
    SELECT 
		u.user_id
    FROM 
      auth."user" u;
    
END;
$$;


ALTER FUNCTION auth.user__get_all_ids() OWNER TO postgres;

--
-- TOC entry 247 (class 1255 OID 17012)
-- Name: user__get_by_email(character varying); Type: FUNCTION; Schema: auth; Owner: postgres
--

CREATE FUNCTION user__get_by_email(p_user_email character varying) RETURNS TABLE(user_id integer, email character varying, email_confirmed bit, pass_hash character varying, salt character varying, created timestamp without time zone, locale_id integer, chosen_place_id integer)
    LANGUAGE plpgsql
    AS $$
DECLARE cur_1 refcursor;
BEGIN

	RETURN QUERY
    SELECT 
		u.user_id,
        u.email,
        u.email_confirmed,
        u.pass_hash,
        u.salt,
        u.created,
        u.locale_id,
  		u.chosen_place_id
    FROM 
      auth."user" u
    WHERE
      LOWER(u.email) = LOWER(p_user_email);
    
END;
$$;


ALTER FUNCTION auth.user__get_by_email(p_user_email character varying) OWNER TO postgres;

--
-- TOC entry 248 (class 1255 OID 17013)
-- Name: user__get_by_id(integer); Type: FUNCTION; Schema: auth; Owner: postgres
--

CREATE FUNCTION user__get_by_id(p_user_id integer) RETURNS TABLE(user_id integer, email character varying, login character varying, pass_hash character varying, pass_salt character varying, date_created timestamp without time zone, date_last_login timestamp without time zone, locale_id integer, chosen_place_id integer)
    LANGUAGE plpgsql
    AS $$
DECLARE cur_1 refcursor;
BEGIN

	RETURN QUERY
    SELECT 
		u.user_id,
        u.email,
        u.login,
        u.pass_hash,
        u.pass_salt,
        u.date_created,
        u.date_last_login,
        u.locale_id,
  		u.chosen_place_id
    FROM 
      auth."user" u
    WHERE
      u.user_id = p_user_id;
    
END;
$$;


ALTER FUNCTION auth.user__get_by_id(p_user_id integer) OWNER TO postgres;

--
-- TOC entry 243 (class 1255 OID 17014)
-- Name: user__get_by_login(character varying); Type: FUNCTION; Schema: auth; Owner: postgres
--

CREATE FUNCTION user__get_by_login(p_user_login character varying) RETURNS TABLE(user_id integer, email character varying, login character varying, pass_hash character varying, pass_salt character varying, date_created timestamp without time zone, date_last_login timestamp without time zone, locale_id integer, chosen_place_id integer)
    LANGUAGE plpgsql
    AS $$
DECLARE cur_1 refcursor;
BEGIN

	RETURN QUERY
    SELECT 
		u.user_id,
        u.email,
        u.login,
        u.pass_hash,
        u.pass_salt,
        u.date_created,
        u.date_last_login,
        u.locale_id,
  		u.chosen_place_id
    FROM 
      auth."user" u
    WHERE
      LOWER(u.login) = LOWER(p_user_login);
    
END;
$$;


ALTER FUNCTION auth.user__get_by_login(p_user_login character varying) OWNER TO postgres;

--
-- TOC entry 237 (class 1255 OID 16562)
-- Name: user__get_claims(integer); Type: FUNCTION; Schema: auth; Owner: postgres
--

CREATE FUNCTION user__get_claims(p_user_id integer) RETURNS TABLE(claim_id integer, claim_code character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN
	RETURN QUERY
  	SELECT c.claim_id
        , c.claim_code
    FROM auth.claim c
        LEFT JOIN auth.user_claim uc
            ON uc.claim_id = c.claim_id
                AND uc.user_id = p_user_id
        LEFT JOIN auth.group_claim gc
            ON gc.claim_id = c.claim_id
        LEFT JOIN auth.group_user gu
            ON gu.group_id = gc.group_id
                AND gu.user_id = p_user_id
	WHERE UC.claim_id IS NOT NULL
    	OR gu.group_id IS NOT NULL;
END;
$$;


ALTER FUNCTION auth.user__get_claims(p_user_id integer) OWNER TO postgres;

--
-- TOC entry 239 (class 1255 OID 16937)
-- Name: user__set_last_login_date(integer, timestamp without time zone); Type: FUNCTION; Schema: auth; Owner: postgres
--

CREATE FUNCTION user__set_last_login_date(p_user_id integer, p_date_last_login timestamp without time zone) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
	
    UPDATE auth.user
    SET date_last_login = p_date_last_login
	WHERE user_id = p_user_id;

END;
$$;


ALTER FUNCTION auth.user__set_last_login_date(p_user_id integer, p_date_last_login timestamp without time zone) OWNER TO postgres;

--
-- TOC entry 222 (class 1255 OID 16561)
-- Name: user_claim__create(integer, integer); Type: FUNCTION; Schema: auth; Owner: postgres
--

CREATE FUNCTION user_claim__create(p_user_id integer, p_claim_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
	
	INSERT INTO auth.user_claim(user_id, claim_id)
    VALUES (p_user_id, p_claim_id);

END;
$$;


ALTER FUNCTION auth.user_claim__create(p_user_id integer, p_claim_id integer) OWNER TO postgres;

--
-- TOC entry 238 (class 1255 OID 16563)
-- Name: user_claim__remove(integer, integer); Type: FUNCTION; Schema: auth; Owner: postgres
--

CREATE FUNCTION user_claim__remove(p_user_id integer, p_claim_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
	
	DELETE FROM auth.user_claim uc
    WHERE uc.user_id = p_user_id
    	AND uc.claim_id = p_claim_id;

END;
$$;


ALTER FUNCTION auth.user_claim__remove(p_user_id integer, p_claim_id integer) OWNER TO postgres;

SET search_path = core, pg_catalog;

--
-- TOC entry 256 (class 1255 OID 17037)
-- Name: locale__get_all(); Type: FUNCTION; Schema: core; Owner: postgres
--

CREATE FUNCTION locale__get_all() RETURNS TABLE(locale_id integer, locale_code character varying, comment character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN

	RETURN QUERY
    SELECT 
    	l.locale_id,
        l.locale_code,
        l.comment
    FROM 
      core.locale l;
    
END;
$$;


ALTER FUNCTION core.locale__get_all() OWNER TO postgres;

--
-- TOC entry 255 (class 1255 OID 17036)
-- Name: locale__get_by_code(character varying); Type: FUNCTION; Schema: core; Owner: postgres
--

CREATE FUNCTION locale__get_by_code(p_code character varying) RETURNS TABLE(locale_id integer, locale_code character varying, comment character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN

	RETURN QUERY
    SELECT 
    	l.locale_id,
        l.locale_code,
        l.comment
    FROM 
      core.locale l
    WHERE l.locale_code = p_code;
    
END;
$$;


ALTER FUNCTION core.locale__get_by_code(p_code character varying) OWNER TO postgres;

--
-- TOC entry 254 (class 1255 OID 17035)
-- Name: locale__get_by_id(integer); Type: FUNCTION; Schema: core; Owner: postgres
--

CREATE FUNCTION locale__get_by_id(p_id integer) RETURNS TABLE(locale_id integer, locale_code character varying, comment character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN

	RETURN QUERY
    SELECT 
    	l.locale_id,
        l.locale_code,
        l.comment
    FROM 
      core.locale l
    WHERE l.locale_id = p_id;
    
END;
$$;


ALTER FUNCTION core.locale__get_by_id(p_id integer) OWNER TO postgres;

--
-- TOC entry 252 (class 1255 OID 17031)
-- Name: object__create(integer, integer, character varying, boolean); Type: FUNCTION; Schema: core; Owner: postgres
--

CREATE FUNCTION object__create(p_parent_object_id integer, p_object_type_id integer, p_object_code character varying, p_is_enabled boolean) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
  INSERT INTO 
    core.object
  (
    parent_object_id,
    object_type_id,
    object_code,
    is_enabled
  )
  VALUES (
    p_parent_object_id,
    p_object_type_id,
    p_object_code,
    p_is_enabled
  );
END;
$$;


ALTER FUNCTION core.object__create(p_parent_object_id integer, p_object_type_id integer, p_object_code character varying, p_is_enabled boolean) OWNER TO postgres;

--
-- TOC entry 220 (class 1255 OID 17032)
-- Name: object__delete(integer); Type: FUNCTION; Schema: core; Owner: postgres
--

CREATE FUNCTION object__delete(p_object_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
	DELETE FROM core.object o
    WHERE o.object_id = p_object_id;
END;
$$;


ALTER FUNCTION core.object__delete(p_object_id integer) OWNER TO postgres;

--
-- TOC entry 251 (class 1255 OID 17025)
-- Name: object__get_all_of_type(integer); Type: FUNCTION; Schema: core; Owner: postgres
--

CREATE FUNCTION object__get_all_of_type(p_object_type_id integer) RETURNS TABLE(object_id integer, parent_object_id integer, object_type_id integer, object_code character varying, is_enabled boolean)
    LANGUAGE plpgsql
    AS $$
BEGIN

	RETURN QUERY
    SELECT 
      o.object_id,
      o.parent_object_id,
      o.object_type_id,
      o.object_code,
      o.is_enabled
    FROM 
      core.object o
    WHERE o.object_type_id = p_object_type_id;
    
END;
$$;


ALTER FUNCTION core.object__get_all_of_type(p_object_type_id integer) OWNER TO postgres;

--
-- TOC entry 258 (class 1255 OID 17042)
-- Name: object__get_by_id(integer); Type: FUNCTION; Schema: core; Owner: postgres
--

CREATE FUNCTION object__get_by_id(p_object_id integer) RETURNS TABLE(object_id integer, parent_object_id integer, object_type_id integer, object_code character varying, is_enabled boolean)
    LANGUAGE plpgsql
    AS $$
BEGIN

	RETURN QUERY
    SELECT 
      o.object_id,
      o.parent_object_id,
      o.object_type_id,
      o.object_code,
      o.is_enabled
    FROM 
      core.object o
    WHERE o.object_id = p_object_id;
    
END;
$$;


ALTER FUNCTION core.object__get_by_id(p_object_id integer) OWNER TO postgres;

--
-- TOC entry 261 (class 1255 OID 17046)
-- Name: object__get_by_type_and_code(integer, character varying); Type: FUNCTION; Schema: core; Owner: postgres
--

CREATE FUNCTION object__get_by_type_and_code(p_type_id integer, p_code character varying) RETURNS TABLE(object_id integer, parent_object_id integer, object_type_id integer, object_code character varying, is_enabled boolean)
    LANGUAGE plpgsql
    AS $$
BEGIN

	RETURN QUERY
    SELECT 
      o.object_id,
      o.parent_object_id,
      o.object_type_id,
      o.object_code,
      o.is_enabled
    FROM 
      core.object o
    WHERE o.object_type_id = p_type_id
    	AND o.object_code = p_code;
    
END;
$$;


ALTER FUNCTION core.object__get_by_type_and_code(p_type_id integer, p_code character varying) OWNER TO postgres;

--
-- TOC entry 253 (class 1255 OID 17034)
-- Name: object__set_code(integer, character varying); Type: FUNCTION; Schema: core; Owner: postgres
--

CREATE FUNCTION object__set_code(p_object_id integer, p_code character varying) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
	UPDATE core.object
	SET object_code = p_code
    WHERE object_id = p_object_id;
END;
$$;


ALTER FUNCTION core.object__set_code(p_object_id integer, p_code character varying) OWNER TO postgres;

--
-- TOC entry 259 (class 1255 OID 17044)
-- Name: object_translation__create(integer, integer, character varying); Type: FUNCTION; Schema: core; Owner: postgres
--

CREATE FUNCTION object_translation__create(p_object_id integer, p_locale_id integer, p_value character varying) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN

	INSERT INTO core.object_translation (
    	object_id,
        locale_id,
        value
    )
    VALUES(
    	p_object_id,
        p_locale_id,
        p_value
    );
    
END;
$$;


ALTER FUNCTION core.object_translation__create(p_object_id integer, p_locale_id integer, p_value character varying) OWNER TO postgres;

--
-- TOC entry 268 (class 1255 OID 17306)
-- Name: object_translation__get_all(); Type: FUNCTION; Schema: core; Owner: postgres
--

CREATE FUNCTION object_translation__get_all() RETURNS TABLE(object_translation_id integer, object_id integer, object_code character varying, locale_id integer, value character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN

	RETURN QUERY
    SELECT 
      ot.object_translation_id,
      ot.object_id,
      ob.object_code,
      ot.locale_id,
      ot.value
    FROM 
      core.object_translation ot
      inner join core.object ob
      	on ob.object_id = ot.object_id;
    
END;
$$;


ALTER FUNCTION core.object_translation__get_all() OWNER TO postgres;

--
-- TOC entry 265 (class 1255 OID 17307)
-- Name: object_translation__get_for_object(integer); Type: FUNCTION; Schema: core; Owner: postgres
--

CREATE FUNCTION object_translation__get_for_object(p_object_id integer) RETURNS TABLE(object_translation_id integer, object_id integer, object_code character varying, locale_id integer, value character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN

	RETURN QUERY
    SELECT 
      ot.object_translation_id,
      ot.object_id,
      ob.object_code,
      ot.locale_id,
      ot.value
    FROM 
      core.object_translation ot
      inner join core.object ob
      	on ob.object_id = ot.object_id
    WHERE
      ot.object_id = p_object_id;
    
END;
$$;


ALTER FUNCTION core.object_translation__get_for_object(p_object_id integer) OWNER TO postgres;

--
-- TOC entry 266 (class 1255 OID 17308)
-- Name: object_translation__get_for_object(integer, integer); Type: FUNCTION; Schema: core; Owner: postgres
--

CREATE FUNCTION object_translation__get_for_object(p_object_id integer, p_locale_id integer) RETURNS TABLE(object_translation_id integer, object_id integer, object_code character varying, locale_id integer, value character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN

	RETURN QUERY
    SELECT 
      ot.object_translation_id,
      ot.object_id,
      ob.object_code,
      ot.locale_id,
      ot.value
    FROM 
      core.object_translation ot
      inner join core.object ob
      	on ob.object_id = ot.object_id
    WHERE
      ot.object_id = p_object_id
      AND ot.locale_id = p_locale_id;
    
END;
$$;


ALTER FUNCTION core.object_translation__get_for_object(p_object_id integer, p_locale_id integer) OWNER TO postgres;

--
-- TOC entry 260 (class 1255 OID 17045)
-- Name: object_translation__set_value(integer, integer, character varying); Type: FUNCTION; Schema: core; Owner: postgres
--

CREATE FUNCTION object_translation__set_value(p_object_id integer, p_locale_id integer, p_value character varying) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN

	UPDATE core.object_translation
    SET
    	value = p_value
    WHERE
    	object_id = p_object_id
        AND locale_id = p_locale_id;
    
END;
$$;


ALTER FUNCTION core.object_translation__set_value(p_object_id integer, p_locale_id integer, p_value character varying) OWNER TO postgres;

--
-- TOC entry 250 (class 1255 OID 17024)
-- Name: object_type__create(character varying); Type: FUNCTION; Schema: core; Owner: postgres
--

CREATE FUNCTION object_type__create(p_code character varying) RETURNS integer
    LANGUAGE plpgsql
    AS $$
DECLARE result INTEGER;
BEGIN

	INSERT INTO core.object_type(
    	object_type_code
    )
	VALUES(
    	p_code
    )
    RETURNING object_type_id into result;
    
    RETURN result;
    
END;
$$;


ALTER FUNCTION core.object_type__create(p_code character varying) OWNER TO postgres;

--
-- TOC entry 223 (class 1255 OID 17022)
-- Name: object_type__get_all(); Type: FUNCTION; Schema: core; Owner: postgres
--

CREATE FUNCTION object_type__get_all() RETURNS TABLE(id integer, code character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN

	RETURN QUERY
    SELECT 
      o.object_type_id,
      o.object_type_code
    FROM 
      core.object_type o;
    
END;
$$;


ALTER FUNCTION core.object_type__get_all() OWNER TO postgres;

--
-- TOC entry 249 (class 1255 OID 17023)
-- Name: object_type__update(integer, character varying); Type: FUNCTION; Schema: core; Owner: postgres
--

CREATE FUNCTION object_type__update(p_id integer, p_code character varying) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN

    UPDATE core.object_type
    SET    object_type_code = p_code
    WHERE  object_type_id = p_id;
    
END;
$$;


ALTER FUNCTION core.object_type__update(p_id integer, p_code character varying) OWNER TO postgres;

SET search_path = dating, pg_catalog;

--
-- TOC entry 273 (class 1255 OID 25507)
-- Name: ad__add(bigint, integer, boolean, timestamp without time zone, timestamp without time zone, character varying, timestamp without time zone, integer, integer, integer, integer, integer, integer, integer, boolean, integer, integer, integer, integer, integer, integer, integer); Type: FUNCTION; Schema: dating; Owner: postgres
--

CREATE FUNCTION ad__add(p_user_id bigint, p_place_id integer, p_is_active boolean, p_date_create timestamp without time zone, p_date_last_modified timestamp without time zone, p_name character varying, p_date_born timestamp without time zone, p_gender_id integer, p_height_cm integer, p_weight_gr integer, p_eye_color_id integer, p_hair_color_id integer, p_hair_length_id integer, p_relationship_status_id integer, p_has_kids boolean, p_education_level_id integer, p_smoking_id integer, p_alcohol_id integer, p_religion_id integer, p_zodiac_sign_id integer, p_body_type_id integer, p_ethnic_group_id integer) RETURNS bigint
    LANGUAGE plpgsql
    AS $$
	DECLARE r_new_id BIGINT;
BEGIN
    INSERT INTO market.ad
    (
      user_id,
      place_id,
      is_active,
      date_create,
      date_last_modified,
      name,
      date_born,
      gender_id,
      height_cm,
      weight_gr,
      eye_color_id,
      hair_color_id,
      hair_length_id,
      relationship_status_id,
      has_kids,
      education_level_id,
      smoking_id,
      alcohol_id,
      religion_id,
      zodiac_sign_id,
      body_type_id,
      ethnic_group_id
    )
    VALUES (
      p_user_id,
      p_place_id,
      p_is_active,
      p_date_create,
      p_date_last_modified,
      p_name,
      p_date_born,
      p_gender_id,
      p_height_cm,
      p_weight_gr,
      p_eye_color_id,
      p_hair_color_id,
      p_hair_length_id,
      p_relationship_status_id,
      p_has_kids,
      p_education_level_id,
      p_smoking_id,
      p_alcohol_id,
      p_religion_id,
      p_zodiac_sign_id,
      p_body_type_id,
      p_ethnic_group_id
    )
    RETURNING ad_id INTO r_new_id;
    
    RETURN r_new_id;
END;
$$;


ALTER FUNCTION dating.ad__add(p_user_id bigint, p_place_id integer, p_is_active boolean, p_date_create timestamp without time zone, p_date_last_modified timestamp without time zone, p_name character varying, p_date_born timestamp without time zone, p_gender_id integer, p_height_cm integer, p_weight_gr integer, p_eye_color_id integer, p_hair_color_id integer, p_hair_length_id integer, p_relationship_status_id integer, p_has_kids boolean, p_education_level_id integer, p_smoking_id integer, p_alcohol_id integer, p_religion_id integer, p_zodiac_sign_id integer, p_body_type_id integer, p_ethnic_group_id integer) OWNER TO postgres;

--
-- TOC entry 274 (class 1255 OID 25508)
-- Name: ad__get(bigint); Type: FUNCTION; Schema: dating; Owner: postgres
--

CREATE FUNCTION ad__get(p_ad_id bigint) RETURNS TABLE(ad_id bigint, user_id bigint, place_id integer, is_active boolean, date_create timestamp without time zone, date_last_modified timestamp without time zone, name character varying, date_born timestamp without time zone, gender_id integer, height_cm integer, weight_gr integer, eye_color_id integer, hair_color_id integer, hair_length_id integer, relationship_status_id integer, has_kids boolean, education_level_id integer, smoking_id integer, alcohol_id integer, religion_id integer, zodiac_sign_id integer, body_type_id integer, ethnic_group_id integer)
    LANGUAGE plpgsql
    AS $$
BEGIN

	RETURN QUERY
    SELECT 
      ad.ad_id,
      ad.user_id,
      ad.place_id,
      ad.is_active,
      ad.date_create,
      ad.date_last_modified,
      ad.name,
      ad.date_born,
      ad.gender_id,
      ad.height_cm,
      ad.weight_gr,
      ad.eye_color_id,
      ad.hair_color_id,
      ad.hair_length_id,
      ad.relationship_status_id,
      ad.has_kids,
      ad.education_level_id,
      ad.smoking_id,
      ad.alcohol_id,
      ad.religion_id,
      ad.zodiac_sign_id,
      ad.body_type_id,
      ad.ethnic_group_id
    FROM 
      market.ad ad
    WHERE
      ad.ad_id = p_ad_id;
    
END;
$$;


ALTER FUNCTION dating.ad__get(p_ad_id bigint) OWNER TO postgres;

--
-- TOC entry 275 (class 1255 OID 25509)
-- Name: ad__get_all(); Type: FUNCTION; Schema: dating; Owner: postgres
--

CREATE FUNCTION ad__get_all() RETURNS TABLE(ad_id bigint, user_id bigint, place_id integer, is_active boolean, date_create timestamp without time zone, date_last_modified timestamp without time zone, name character varying, date_born timestamp without time zone, gender_id integer, height_cm integer, weight_gr integer, eye_color_id integer, hair_color_id integer, hair_length_id integer, relationship_status_id integer, has_kids boolean, education_level_id integer, smoking_id integer, alcohol_id integer, religion_id integer, zodiac_sign_id integer, body_type_id integer, ethnic_group_id integer)
    LANGUAGE plpgsql
    AS $$
BEGIN

	RETURN QUERY
    SELECT 
        ad.ad_id,
        ad.user_id,
        ad.place_id,
        ad.is_active,
        ad.date_create,
        ad.date_last_modified,
        ad.name,
        ad.date_born,
        ad.gender_id,
        ad.height_cm,
        ad.weight_gr,
        ad.eye_color_id,
        ad.hair_color_id,
        ad.hair_length_id,
        ad.relationship_status_id,
        ad.has_kids,
        ad.education_level_id,
        ad.smoking_id,
        ad.alcohol_id,
        ad.religion_id,
        ad.zodiac_sign_id,
        ad.body_type_id,
        ad.ethnic_group_id
      FROM 
        dating.ad ad;
    
END;
$$;


ALTER FUNCTION dating.ad__get_all() OWNER TO postgres;

--
-- TOC entry 276 (class 1255 OID 25510)
-- Name: ad__get_by_user(bigint); Type: FUNCTION; Schema: dating; Owner: postgres
--

CREATE FUNCTION ad__get_by_user(p_user_id bigint) RETURNS TABLE(ad_id bigint, user_id bigint, place_id integer, is_active boolean, date_create timestamp without time zone, date_last_modified timestamp without time zone, name character varying, date_born timestamp without time zone, gender_id integer, height_cm integer, weight_gr integer, eye_color_id integer, hair_color_id integer, hair_length_id integer, relationship_status_id integer, has_kids boolean, education_level_id integer, smoking_id integer, alcohol_id integer, religion_id integer, zodiac_sign_id integer, body_type_id integer, ethnic_group_id integer)
    LANGUAGE plpgsql
    AS $$
BEGIN

	RETURN QUERY
    SELECT 
      ad.ad_id,
      ad.user_id,
      ad.place_id,
      ad.is_active,
      ad.date_create,
      ad.date_last_modified,
      ad.name,
      ad.date_born,
      ad.gender_id,
      ad.height_cm,
      ad.weight_gr,
      ad.eye_color_id,
      ad.hair_color_id,
      ad.hair_length_id,
      ad.relationship_status_id,
      ad.has_kids,
      ad.education_level_id,
      ad.smoking_id,
      ad.alcohol_id,
      ad.religion_id,
      ad.zodiac_sign_id,
      ad.body_type_id,
      ad.ethnic_group_id
    FROM 
      market.ad ad
    WHERE
      ad.user_id = p_user_id;
    
END;
$$;


ALTER FUNCTION dating.ad__get_by_user(p_user_id bigint) OWNER TO postgres;

--
-- TOC entry 272 (class 1255 OID 25511)
-- Name: ad__update(bigint, bigint, integer, boolean, timestamp without time zone, timestamp without time zone, character varying, timestamp without time zone, integer, integer, integer, integer, integer, integer, integer, boolean, integer, integer, integer, integer, integer, integer, integer); Type: FUNCTION; Schema: dating; Owner: postgres
--

CREATE FUNCTION ad__update(p_ad_id bigint, p_user_id bigint, p_place_id integer, p_is_active boolean, p_date_create timestamp without time zone, p_date_last_modified timestamp without time zone, p_name character varying, p_date_born timestamp without time zone, p_gender_id integer, p_height_cm integer, p_weight_gr integer, p_eye_color_id integer, p_hair_color_id integer, p_hair_length_id integer, p_relationship_status_id integer, p_has_kids boolean, p_education_level_id integer, p_smoking_id integer, p_alcohol_id integer, p_religion_id integer, p_zodiac_sign_id integer, p_body_type_id integer, p_ethnic_group_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN

	UPDATE market.ad
    SET
      user_id = p_user_id,
      place_id = p_place_id,
      is_active = p_is_active,
      date_create = p_date_create,
      date_last_modified = p_date_last_modified,
      name = p_name,
      date_born = p_date_born,
      gender_id = p_gender_id,
      height_cm = p_height_cm,
      weight_gr = p_weight_gr,
      eye_color_id = p_eye_color_id,
      hair_color_id = p_hair_color_id,
      hair_length_id = p_hair_length_id,
      relationship_status_id = p_relationship_status_id,
      has_kids = p_has_kids,
      education_level_id = p_education_level_id,
      smoking_id = p_smoking_id,
      alcohol_id = p_alcohol_id,
      religion_id = p_religion_id,
      zodiac_sign_id = p_zodiac_sign_id,
      body_type_id = p_body_type_id,
      ethnic_group_id = p_ethnic_group_id
    WHERE ad_id = p_ad_id;
    
END;
$$;


ALTER FUNCTION dating.ad__update(p_ad_id bigint, p_user_id bigint, p_place_id integer, p_is_active boolean, p_date_create timestamp without time zone, p_date_last_modified timestamp without time zone, p_name character varying, p_date_born timestamp without time zone, p_gender_id integer, p_height_cm integer, p_weight_gr integer, p_eye_color_id integer, p_hair_color_id integer, p_hair_length_id integer, p_relationship_status_id integer, p_has_kids boolean, p_education_level_id integer, p_smoking_id integer, p_alcohol_id integer, p_religion_id integer, p_zodiac_sign_id integer, p_body_type_id integer, p_ethnic_group_id integer) OWNER TO postgres;

--
-- TOC entry 269 (class 1255 OID 17255)
-- Name: ad_media__add(bigint, integer, timestamp without time zone, boolean, integer, character varying); Type: FUNCTION; Schema: dating; Owner: postgres
--

CREATE FUNCTION ad_media__add(p_ad_id bigint, p_media_type_id integer, p_date_create timestamp without time zone, p_is_primary boolean, p_position integer, p_original_file_name character varying) RETURNS bigint
    LANGUAGE plpgsql
    AS $$
DECLARE
  r_new_id BIGINT;
BEGIN

	  INSERT INTO dating.ad_media
      (
        ad_id,
        media_type_id,
        date_create,
        is_primary,
        position,
        original_file_name
      )
      VALUES (
        p_ad_id ,
        p_media_type_id ,
        p_date_create ,
        p_is_primary ,
        p_position ,
        p_original_file_name
      )
      RETURNING ad_media_id INTO r_new_id;
      
      RETURN r_new_id;
      
END;
$$;


ALTER FUNCTION dating.ad_media__add(p_ad_id bigint, p_media_type_id integer, p_date_create timestamp without time zone, p_is_primary boolean, p_position integer, p_original_file_name character varying) OWNER TO postgres;

--
-- TOC entry 262 (class 1255 OID 17269)
-- Name: ad_media__assign_to_ad(bigint, bigint, boolean, integer); Type: FUNCTION; Schema: dating; Owner: postgres
--

CREATE FUNCTION ad_media__assign_to_ad(p_ad_media_id bigint, p_ad_id bigint, p_is_primary boolean, p_position integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
DECLARE
BEGIN

	UPDATE dating.ad_media
    SET
    	ad_id = p_ad_id,
        is_primary = p_is_primary,
        position = p_position
    WHERE
    	ad_media_id = p_ad_media_id;
      
END;
$$;


ALTER FUNCTION dating.ad_media__assign_to_ad(p_ad_media_id bigint, p_ad_id bigint, p_is_primary boolean, p_position integer) OWNER TO postgres;

--
-- TOC entry 267 (class 1255 OID 17259)
-- Name: ad_media__get(bigint); Type: FUNCTION; Schema: dating; Owner: postgres
--

CREATE FUNCTION ad_media__get(p_ad_media_id bigint) RETURNS TABLE(ad_media_id bigint, ad_id bigint, media_type_id integer, date_create timestamp without time zone, is_primary boolean, "position" integer, original_file_name character varying)
    LANGUAGE plpgsql
    AS $$
DECLARE
BEGIN

	  RETURN QUERY
      SELECT 
        am.ad_media_id,
        am.ad_id,
        am.media_type_id,
        am.date_create,
        am.is_primary,
        am."position",
        am.original_file_name
      FROM 
     	 dating.ad_media am
      WHERE am.ad_media_id = p_ad_media_id;
      
END;
$$;


ALTER FUNCTION dating.ad_media__get(p_ad_media_id bigint) OWNER TO postgres;

--
-- TOC entry 257 (class 1255 OID 17262)
-- Name: ad_media__get_by_ad_id(bigint); Type: FUNCTION; Schema: dating; Owner: postgres
--

CREATE FUNCTION ad_media__get_by_ad_id(p_ad_id bigint) RETURNS TABLE(ad_media_id bigint, ad_id bigint, media_type_id integer, date_create timestamp without time zone, is_primary boolean, "position" integer, original_file_name character varying)
    LANGUAGE plpgsql
    AS $$
DECLARE
BEGIN

	  RETURN QUERY
      SELECT 
        am.ad_media_id,
        am.ad_id,
        am.media_type_id,
        am.date_create,
        am.is_primary,
        am."position",
        am.original_file_name
      FROM 
     	 dating.ad_media am
      WHERE am.ad_id = p_ad_id;
      
END;
$$;


ALTER FUNCTION dating.ad_media__get_by_ad_id(p_ad_id bigint) OWNER TO postgres;

--
-- TOC entry 270 (class 1255 OID 17258)
-- Name: ad_media__remove(bigint); Type: FUNCTION; Schema: dating; Owner: postgres
--

CREATE FUNCTION ad_media__remove(p_ad_media_id bigint) RETURNS void
    LANGUAGE plpgsql
    AS $$
DECLARE
BEGIN

      DELETE 
      FROM dating.ad_media
      WHERE ad_media_id = p_ad_media_id;
      
END;
$$;


ALTER FUNCTION dating.ad_media__remove(p_ad_media_id bigint) OWNER TO postgres;

--
-- TOC entry 263 (class 1255 OID 17270)
-- Name: ad_media_pic_data__add(bigint, integer, integer, character varying, character varying); Type: FUNCTION; Schema: dating; Owner: postgres
--

CREATE FUNCTION ad_media_pic_data__add(p_ad_media_id bigint, p_width integer, p_height integer, p_relative_path character varying, p_pic_type character varying) RETURNS bigint
    LANGUAGE plpgsql
    AS $$
DECLARE
  r_new_id BIGINT;
BEGIN

      INSERT INTO dating.ad_media_pic_data
      (
        ad_media_id,
        width,
        height,
        relative_path,
        pic_type
      )
      VALUES (
        p_ad_media_id,
        p_width,
        p_height,
        p_relative_path,
        p_pic_type
      )
      RETURNING ad_media_pic_data_id INTO r_new_id;
      
      RETURN r_new_id;
      
END;
$$;


ALTER FUNCTION dating.ad_media_pic_data__add(p_ad_media_id bigint, p_width integer, p_height integer, p_relative_path character varying, p_pic_type character varying) OWNER TO postgres;

--
-- TOC entry 264 (class 1255 OID 17271)
-- Name: ad_media_pic_data__get_by_ad_media_id(bigint); Type: FUNCTION; Schema: dating; Owner: postgres
--

CREATE FUNCTION ad_media_pic_data__get_by_ad_media_id(p_ad_media_id bigint) RETURNS TABLE(ad_media_pic_data_id bigint, ad_media_id bigint, width integer, height integer, relative_path character varying, pic_type character varying)
    LANGUAGE plpgsql
    AS $$
DECLARE
BEGIN

	  RETURN QUERY
      SELECT 
        ampd.ad_media_pic_data_id,
        ampd.ad_media_id,
        ampd.width,
        ampd.height,
        ampd.relative_path,
        ampd.pic_type
      FROM 
        dating.ad_media_pic_data ampd
      WHERE ampd.ad_media_id = p_ad_media_id;
      
END;
$$;


ALTER FUNCTION dating.ad_media_pic_data__get_by_ad_media_id(p_ad_media_id bigint) OWNER TO postgres;

--
-- TOC entry 271 (class 1255 OID 17257)
-- Name: ad_media_pic_data__remove_by_ad_media_id(bigint); Type: FUNCTION; Schema: dating; Owner: postgres
--

CREATE FUNCTION ad_media_pic_data__remove_by_ad_media_id(p_ad_media_id bigint) RETURNS void
    LANGUAGE plpgsql
    AS $$
DECLARE
BEGIN

      DELETE 
      FROM dating.ad_media_pic_data
      WHERE ad_media_id = p_ad_media_id;
      
END;
$$;


ALTER FUNCTION dating.ad_media_pic_data__remove_by_ad_media_id(p_ad_media_id bigint) OWNER TO postgres;

SET search_path = geo, pg_catalog;

--
-- TOC entry 244 (class 1255 OID 17005)
-- Name: place__create(integer, character varying, integer, boolean); Type: FUNCTION; Schema: geo; Owner: postgres
--

CREATE FUNCTION place__create(p_parent_place_id integer, p_place_code character varying, p_place_type_id integer, p_is_enabled boolean) RETURNS integer
    LANGUAGE plpgsql
    AS $$
DECLARE result INTEGER;
BEGIN

	INSERT INTO geo.place(
    	parent_place_id
    	, place_code
        , place_type_id
        , is_enabled)
	VALUES(
      p_parent_place_id,
      p_place_code,
      p_place_type_id,
      p_is_enabled
    )
    RETURNING place_id into result;
    
    RETURN result;
    
END;
$$;


ALTER FUNCTION geo.place__create(p_parent_place_id integer, p_place_code character varying, p_place_type_id integer, p_is_enabled boolean) OWNER TO postgres;

--
-- TOC entry 242 (class 1255 OID 17000)
-- Name: place__get(integer); Type: FUNCTION; Schema: geo; Owner: postgres
--

CREATE FUNCTION place__get(p_place_id integer) RETURNS TABLE(place_id integer, parent_place_id integer, place_code character varying, place_type_id integer, is_enabled boolean)
    LANGUAGE plpgsql
    AS $$
BEGIN

	RETURN QUERY
    SELECT 
      p.place_id,
      p.parent_place_id,
      p.place_code,
      p.place_type_id,
      p.is_enabled
    FROM 
      geo.place p
    where p.place_id = p_place_id;
    
END;
$$;


ALTER FUNCTION geo.place__get(p_place_id integer) OWNER TO postgres;

--
-- TOC entry 240 (class 1255 OID 16997)
-- Name: place__get_all(); Type: FUNCTION; Schema: geo; Owner: postgres
--

CREATE FUNCTION place__get_all() RETURNS TABLE(place_id integer, parent_place_id integer, place_code character varying, place_type_id integer, is_enabled boolean)
    LANGUAGE plpgsql
    AS $$
BEGIN

	RETURN QUERY
    SELECT 
      p.place_id,
      p.parent_place_id,
      p.place_code,
      p.place_type_id,
      p.is_enabled
    FROM 
      geo.place p;
    
END;
$$;


ALTER FUNCTION geo.place__get_all() OWNER TO postgres;

--
-- TOC entry 246 (class 1255 OID 17001)
-- Name: place__update(integer, integer, character varying, integer, boolean); Type: FUNCTION; Schema: geo; Owner: postgres
--

CREATE FUNCTION place__update(p_place_id integer, p_parent_place_id integer, p_place_code character varying, p_place_type_id integer, p_is_enabled boolean) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN

	UPDATE geo.place
    SET parent_place_id = p_parent_place_id,
		place_code = p_place_code,
        place_type_id = p_place_type_id,
        is_enabled = p_is_enabled
    WHERE place_id = p_place_id;
    
END;
$$;


ALTER FUNCTION geo.place__update(p_place_id integer, p_parent_place_id integer, p_place_code character varying, p_place_type_id integer, p_is_enabled boolean) OWNER TO postgres;

SET search_path = auth, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 189 (class 1259 OID 16566)
-- Name: claim; Type: TABLE; Schema: auth; Owner: postgres
--

CREATE TABLE claim (
    claim_id integer DEFAULT nextval(('auth.claim__claim_id_seq'::text)::regclass) NOT NULL,
    claim_code character varying(20) NOT NULL
);


ALTER TABLE claim OWNER TO postgres;

--
-- TOC entry 194 (class 1259 OID 16591)
-- Name: claim__claim_id_seq; Type: SEQUENCE; Schema: auth; Owner: postgres
--

CREATE SEQUENCE claim__claim_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    MAXVALUE 2147483647
    CACHE 1;


ALTER TABLE claim__claim_id_seq OWNER TO postgres;

--
-- TOC entry 190 (class 1259 OID 16570)
-- Name: group; Type: TABLE; Schema: auth; Owner: postgres
--

CREATE TABLE "group" (
    group_id integer DEFAULT nextval(('auth.group__group_id_seq'::text)::regclass) NOT NULL,
    group_name character varying(50) NOT NULL
);


ALTER TABLE "group" OWNER TO postgres;

--
-- TOC entry 195 (class 1259 OID 16593)
-- Name: group__group_id_seq; Type: SEQUENCE; Schema: auth; Owner: postgres
--

CREATE SEQUENCE group__group_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    MAXVALUE 2147483647
    CACHE 1;


ALTER TABLE group__group_id_seq OWNER TO postgres;

--
-- TOC entry 193 (class 1259 OID 16588)
-- Name: group_claim; Type: TABLE; Schema: auth; Owner: postgres
--

CREATE TABLE group_claim (
    group_id integer NOT NULL,
    claim_id integer NOT NULL
);
ALTER TABLE ONLY group_claim ALTER COLUMN group_id SET STATISTICS 0;


ALTER TABLE group_claim OWNER TO postgres;

--
-- TOC entry 191 (class 1259 OID 16582)
-- Name: group_user; Type: TABLE; Schema: auth; Owner: postgres
--

CREATE TABLE group_user (
    group_id integer NOT NULL,
    user_id integer NOT NULL
);
ALTER TABLE ONLY group_user ALTER COLUMN group_id SET STATISTICS 0;


ALTER TABLE group_user OWNER TO postgres;

--
-- TOC entry 197 (class 1259 OID 16639)
-- Name: user; Type: TABLE; Schema: auth; Owner: postgres
--

CREATE TABLE "user" (
    user_id integer DEFAULT nextval(('auth.user__user_id_seq'::text)::regclass) NOT NULL,
    email character varying(50) NOT NULL,
    login character varying(50) NOT NULL,
    pass_hash character varying(256) NOT NULL,
    pass_salt character varying(256) NOT NULL,
    date_created timestamp(6) without time zone DEFAULT now() NOT NULL,
    date_last_login timestamp(6) without time zone,
    locale_id integer,
    chosen_place_id integer
);


ALTER TABLE "user" OWNER TO postgres;

--
-- TOC entry 196 (class 1259 OID 16595)
-- Name: user__user_id_seq; Type: SEQUENCE; Schema: auth; Owner: postgres
--

CREATE SEQUENCE user__user_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    MAXVALUE 2147483647
    CACHE 1;


ALTER TABLE user__user_id_seq OWNER TO postgres;

--
-- TOC entry 192 (class 1259 OID 16585)
-- Name: user_claim; Type: TABLE; Schema: auth; Owner: postgres
--

CREATE TABLE user_claim (
    claim_id integer NOT NULL,
    user_id integer NOT NULL
);
ALTER TABLE ONLY user_claim ALTER COLUMN claim_id SET STATISTICS 0;


ALTER TABLE user_claim OWNER TO postgres;

SET search_path = core, pg_catalog;

--
-- TOC entry 205 (class 1259 OID 16887)
-- Name: locale_id_seq; Type: SEQUENCE; Schema: core; Owner: postgres
--

CREATE SEQUENCE locale_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE locale_id_seq OWNER TO postgres;

--
-- TOC entry 206 (class 1259 OID 16889)
-- Name: locale; Type: TABLE; Schema: core; Owner: postgres
--

CREATE TABLE locale (
    locale_id integer DEFAULT nextval('locale_id_seq'::regclass) NOT NULL,
    locale_code character varying(6) NOT NULL,
    comment character varying(64)
);


ALTER TABLE locale OWNER TO postgres;

--
-- TOC entry 212 (class 1259 OID 17026)
-- Name: object_id_seq; Type: SEQUENCE; Schema: core; Owner: postgres
--

CREATE SEQUENCE object_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE object_id_seq OWNER TO postgres;

--
-- TOC entry 210 (class 1259 OID 16981)
-- Name: object; Type: TABLE; Schema: core; Owner: postgres
--

CREATE TABLE object (
    object_id integer DEFAULT nextval('object_id_seq'::regclass) NOT NULL,
    parent_object_id integer,
    object_type_id integer NOT NULL,
    object_code character varying(100) NOT NULL,
    is_enabled boolean
);


ALTER TABLE object OWNER TO postgres;

--
-- TOC entry 207 (class 1259 OID 16897)
-- Name: string_id_seq; Type: SEQUENCE; Schema: core; Owner: postgres
--

CREATE SEQUENCE string_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE string_id_seq OWNER TO postgres;

--
-- TOC entry 208 (class 1259 OID 16902)
-- Name: object_translation; Type: TABLE; Schema: core; Owner: postgres
--

CREATE TABLE object_translation (
    object_translation_id integer DEFAULT nextval('string_id_seq'::regclass) NOT NULL,
    object_id integer NOT NULL,
    locale_id integer NOT NULL,
    value character varying NOT NULL
);


ALTER TABLE object_translation OWNER TO postgres;

--
-- TOC entry 203 (class 1259 OID 16877)
-- Name: object_type_id_seq; Type: SEQUENCE; Schema: core; Owner: postgres
--

CREATE SEQUENCE object_type_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE object_type_id_seq OWNER TO postgres;

--
-- TOC entry 204 (class 1259 OID 16879)
-- Name: object_type; Type: TABLE; Schema: core; Owner: postgres
--

CREATE TABLE object_type (
    object_type_id integer DEFAULT nextval('object_type_id_seq'::regclass) NOT NULL,
    object_type_code character varying(64) NOT NULL
);


ALTER TABLE object_type OWNER TO postgres;

SET search_path = dating, pg_catalog;

--
-- TOC entry 217 (class 1259 OID 17250)
-- Name: ad__ad_id__seq; Type: SEQUENCE; Schema: dating; Owner: postgres
--

CREATE SEQUENCE ad__ad_id__seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE ad__ad_id__seq OWNER TO postgres;

--
-- TOC entry 199 (class 1259 OID 16676)
-- Name: ad; Type: TABLE; Schema: dating; Owner: postgres
--

CREATE TABLE ad (
    ad_id bigint DEFAULT nextval('ad__ad_id__seq'::regclass) NOT NULL,
    user_id bigint NOT NULL,
    place_id integer NOT NULL,
    is_active boolean NOT NULL,
    date_create timestamp(0) without time zone NOT NULL,
    date_last_modified timestamp(0) without time zone,
    name character varying(50),
    date_born timestamp(0) without time zone,
    gender_id integer,
    height_cm integer,
    weight_gr integer,
    eye_color_id integer,
    hair_color_id integer,
    hair_length_id integer,
    relationship_status_id integer,
    has_kids boolean,
    education_level_id integer,
    smoking_id integer,
    alcohol_id integer,
    religion_id integer,
    zodiac_sign_id integer,
    body_type_id integer,
    ethnic_group_id integer
);
ALTER TABLE ONLY ad ALTER COLUMN place_id SET STATISTICS 0;
ALTER TABLE ONLY ad ALTER COLUMN is_active SET STATISTICS 0;
ALTER TABLE ONLY ad ALTER COLUMN date_create SET STATISTICS 0;
ALTER TABLE ONLY ad ALTER COLUMN date_last_modified SET STATISTICS 0;


ALTER TABLE ad OWNER TO postgres;

--
-- TOC entry 209 (class 1259 OID 16947)
-- Name: ad__to__perk_object; Type: TABLE; Schema: dating; Owner: postgres
--

CREATE TABLE ad__to__perk_object (
    ad_id integer NOT NULL,
    perk_object_id integer NOT NULL
);


ALTER TABLE ad__to__perk_object OWNER TO postgres;

--
-- TOC entry 216 (class 1259 OID 17244)
-- Name: ad_media__id_seq; Type: SEQUENCE; Schema: dating; Owner: postgres
--

CREATE SEQUENCE ad_media__id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE ad_media__id_seq OWNER TO postgres;

--
-- TOC entry 214 (class 1259 OID 17231)
-- Name: ad_media; Type: TABLE; Schema: dating; Owner: postgres
--

CREATE TABLE ad_media (
    ad_media_id bigint DEFAULT nextval('ad_media__id_seq'::regclass) NOT NULL,
    ad_id bigint,
    media_type_id integer NOT NULL,
    date_create timestamp(0) without time zone NOT NULL,
    is_primary boolean NOT NULL,
    "position" integer NOT NULL,
    original_file_name character varying(500)
);


ALTER TABLE ad_media OWNER TO postgres;

--
-- TOC entry 215 (class 1259 OID 17242)
-- Name: ad_media_pic_data__id_seq; Type: SEQUENCE; Schema: dating; Owner: postgres
--

CREATE SEQUENCE ad_media_pic_data__id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE ad_media_pic_data__id_seq OWNER TO postgres;

--
-- TOC entry 213 (class 1259 OID 17137)
-- Name: ad_media_pic_data; Type: TABLE; Schema: dating; Owner: postgres
--

CREATE TABLE ad_media_pic_data (
    ad_media_pic_data_id bigint DEFAULT nextval('ad_media_pic_data__id_seq'::regclass) NOT NULL,
    ad_media_id bigint NOT NULL,
    width integer NOT NULL,
    height integer NOT NULL,
    relative_path character varying(500) NOT NULL,
    pic_type character varying(20) NOT NULL
);


ALTER TABLE ad_media_pic_data OWNER TO postgres;

--
-- TOC entry 211 (class 1259 OID 17015)
-- Name: ad_status; Type: TABLE; Schema: dating; Owner: postgres
--

CREATE TABLE ad_status (
    ad_status_id integer NOT NULL,
    status_code character varying(20) NOT NULL
);


ALTER TABLE ad_status OWNER TO postgres;

SET search_path = geo, pg_catalog;

--
-- TOC entry 201 (class 1259 OID 16851)
-- Name: place_id_seq; Type: SEQUENCE; Schema: geo; Owner: postgres
--

CREATE SEQUENCE place_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE place_id_seq OWNER TO postgres;

SET default_tablespace = geo_db;

--
-- TOC entry 200 (class 1259 OID 16846)
-- Name: place; Type: TABLE; Schema: geo; Owner: postgres; Tablespace: geo_db
--

CREATE TABLE place (
    place_id integer DEFAULT nextval('place_id_seq'::regclass) NOT NULL,
    parent_place_id integer,
    place_code character varying(64) NOT NULL,
    place_type_id integer NOT NULL,
    is_enabled boolean NOT NULL
);


ALTER TABLE place OWNER TO postgres;

SET default_tablespace = '';

--
-- TOC entry 202 (class 1259 OID 16859)
-- Name: place_type; Type: TABLE; Schema: geo; Owner: postgres
--

CREATE TABLE place_type (
    place_type_id integer NOT NULL,
    place_type_code character varying(64) NOT NULL
);


ALTER TABLE place_type OWNER TO postgres;

SET search_path = register, pg_catalog;

--
-- TOC entry 198 (class 1259 OID 16665)
-- Name: user_registration_request; Type: TABLE; Schema: register; Owner: postgres
--

CREATE TABLE user_registration_request (
    request_id uuid NOT NULL,
    email character varying(50) NOT NULL,
    token character varying(50) NOT NULL,
    request_dt timestamp(6) without time zone NOT NULL
);


ALTER TABLE user_registration_request OWNER TO postgres;

SET search_path = auth, pg_catalog;

--
-- TOC entry 2158 (class 2606 OID 16598)
-- Name: claim claim_code_key; Type: CONSTRAINT; Schema: auth; Owner: postgres
--

ALTER TABLE ONLY claim
    ADD CONSTRAINT claim_code_key UNIQUE (claim_code);


--
-- TOC entry 2160 (class 2606 OID 16604)
-- Name: claim claim_pkey; Type: CONSTRAINT; Schema: auth; Owner: postgres
--

ALTER TABLE ONLY claim
    ADD CONSTRAINT claim_pkey PRIMARY KEY (claim_id);


--
-- TOC entry 2162 (class 2606 OID 16600)
-- Name: group group_group_name_key; Type: CONSTRAINT; Schema: auth; Owner: postgres
--

ALTER TABLE ONLY "group"
    ADD CONSTRAINT group_group_name_key UNIQUE (group_name);


--
-- TOC entry 2164 (class 2606 OID 16616)
-- Name: group group_pkey; Type: CONSTRAINT; Schema: auth; Owner: postgres
--

ALTER TABLE ONLY "group"
    ADD CONSTRAINT group_pkey PRIMARY KEY (group_id);


--
-- TOC entry 2166 (class 2606 OID 16650)
-- Name: user user_email_key; Type: CONSTRAINT; Schema: auth; Owner: postgres
--

ALTER TABLE ONLY "user"
    ADD CONSTRAINT user_email_key UNIQUE (email);


--
-- TOC entry 2168 (class 2606 OID 16652)
-- Name: user user_login_key; Type: CONSTRAINT; Schema: auth; Owner: postgres
--

ALTER TABLE ONLY "user"
    ADD CONSTRAINT user_login_key UNIQUE (login);


--
-- TOC entry 2170 (class 2606 OID 16648)
-- Name: user user_pkey; Type: CONSTRAINT; Schema: auth; Owner: postgres
--

ALTER TABLE ONLY "user"
    ADD CONSTRAINT user_pkey PRIMARY KEY (user_id);


SET search_path = core, pg_catalog;

--
-- TOC entry 2190 (class 2606 OID 16896)
-- Name: locale locale_locale_code_key; Type: CONSTRAINT; Schema: core; Owner: postgres
--

ALTER TABLE ONLY locale
    ADD CONSTRAINT locale_locale_code_key UNIQUE (locale_code);


--
-- TOC entry 2192 (class 2606 OID 16894)
-- Name: locale locale_pkey; Type: CONSTRAINT; Schema: core; Owner: postgres
--

ALTER TABLE ONLY locale
    ADD CONSTRAINT locale_pkey PRIMARY KEY (locale_id);


--
-- TOC entry 2196 (class 2606 OID 16985)
-- Name: object object_pkey; Type: CONSTRAINT; Schema: core; Owner: postgres
--

ALTER TABLE ONLY object
    ADD CONSTRAINT object_pkey PRIMARY KEY (object_id);


--
-- TOC entry 2194 (class 2606 OID 17040)
-- Name: object_translation object_translation_pkey; Type: CONSTRAINT; Schema: core; Owner: postgres
--

ALTER TABLE ONLY object_translation
    ADD CONSTRAINT object_translation_pkey PRIMARY KEY (object_translation_id);


--
-- TOC entry 2186 (class 2606 OID 16886)
-- Name: object_type object_type_object_type_code_key; Type: CONSTRAINT; Schema: core; Owner: postgres
--

ALTER TABLE ONLY object_type
    ADD CONSTRAINT object_type_object_type_code_key UNIQUE (object_type_code);


--
-- TOC entry 2188 (class 2606 OID 16884)
-- Name: object_type object_type_pkey; Type: CONSTRAINT; Schema: core; Owner: postgres
--

ALTER TABLE ONLY object_type
    ADD CONSTRAINT object_type_pkey PRIMARY KEY (object_type_id);


SET search_path = dating, pg_catalog;

--
-- TOC entry 2199 (class 2606 OID 17019)
-- Name: ad_status ad_status_pkey; Type: CONSTRAINT; Schema: dating; Owner: postgres
--

ALTER TABLE ONLY ad_status
    ADD CONSTRAINT ad_status_pkey PRIMARY KEY (ad_status_id);


--
-- TOC entry 2201 (class 2606 OID 17021)
-- Name: ad_status ad_status_status_code_key; Type: CONSTRAINT; Schema: dating; Owner: postgres
--

ALTER TABLE ONLY ad_status
    ADD CONSTRAINT ad_status_status_code_key UNIQUE (status_code);


--
-- TOC entry 2178 (class 2606 OID 16683)
-- Name: ad dating_ad_pkey; Type: CONSTRAINT; Schema: dating; Owner: postgres
--

ALTER TABLE ONLY ad
    ADD CONSTRAINT dating_ad_pkey PRIMARY KEY (ad_id);


SET search_path = geo, pg_catalog;

--
-- TOC entry 2180 (class 2606 OID 16850)
-- Name: place place_pkey; Type: CONSTRAINT; Schema: geo; Owner: postgres
--

ALTER TABLE ONLY place
    ADD CONSTRAINT place_pkey PRIMARY KEY (place_id);


--
-- TOC entry 2182 (class 2606 OID 16863)
-- Name: place_type place_type_pkey; Type: CONSTRAINT; Schema: geo; Owner: postgres
--

ALTER TABLE ONLY place_type
    ADD CONSTRAINT place_type_pkey PRIMARY KEY (place_type_id);


--
-- TOC entry 2184 (class 2606 OID 16939)
-- Name: place_type place_type_place_type_code_key; Type: CONSTRAINT; Schema: geo; Owner: postgres
--

ALTER TABLE ONLY place_type
    ADD CONSTRAINT place_type_place_type_code_key UNIQUE (place_type_code);


SET search_path = register, pg_catalog;

--
-- TOC entry 2172 (class 2606 OID 16671)
-- Name: user_registration_request user_registration_request_email_key; Type: CONSTRAINT; Schema: register; Owner: postgres
--

ALTER TABLE ONLY user_registration_request
    ADD CONSTRAINT user_registration_request_email_key UNIQUE (email);


--
-- TOC entry 2174 (class 2606 OID 16669)
-- Name: user_registration_request user_registration_request_pkey; Type: CONSTRAINT; Schema: register; Owner: postgres
--

ALTER TABLE ONLY user_registration_request
    ADD CONSTRAINT user_registration_request_pkey PRIMARY KEY (request_id);


--
-- TOC entry 2176 (class 2606 OID 16673)
-- Name: user_registration_request user_registration_request_token_key; Type: CONSTRAINT; Schema: register; Owner: postgres
--

ALTER TABLE ONLY user_registration_request
    ADD CONSTRAINT user_registration_request_token_key UNIQUE (token);


SET search_path = core, pg_catalog;

--
-- TOC entry 2197 (class 1259 OID 16991)
-- Name: object_type_id_idx; Type: INDEX; Schema: core; Owner: postgres
--

CREATE INDEX object_type_id_idx ON object USING btree (object_type_id);


SET search_path = auth, pg_catalog;

--
-- TOC entry 2206 (class 2606 OID 16622)
-- Name: group_claim group_claim_group_id_fkey; Type: FK CONSTRAINT; Schema: auth; Owner: postgres
--

ALTER TABLE ONLY group_claim
    ADD CONSTRAINT group_claim_group_id_fkey FOREIGN KEY (group_id) REFERENCES "group"(group_id);


--
-- TOC entry 2202 (class 2606 OID 16617)
-- Name: group_user group_to_user_group_id_fkey; Type: FK CONSTRAINT; Schema: auth; Owner: postgres
--

ALTER TABLE ONLY group_user
    ADD CONSTRAINT group_to_user_group_id_fkey FOREIGN KEY (group_id) REFERENCES "group"(group_id);


--
-- TOC entry 2205 (class 2606 OID 16610)
-- Name: group_claim group_user_claim_id_fkey; Type: FK CONSTRAINT; Schema: auth; Owner: postgres
--

ALTER TABLE ONLY group_claim
    ADD CONSTRAINT group_user_claim_id_fkey FOREIGN KEY (claim_id) REFERENCES claim(claim_id);


--
-- TOC entry 2208 (class 2606 OID 17006)
-- Name: user user_chosen_place_id_fk; Type: FK CONSTRAINT; Schema: auth; Owner: postgres
--

ALTER TABLE ONLY "user"
    ADD CONSTRAINT user_chosen_place_id_fk FOREIGN KEY (chosen_place_id) REFERENCES geo.place(place_id);


--
-- TOC entry 2203 (class 2606 OID 16605)
-- Name: user_claim user_claim__claim_id_fkey; Type: FK CONSTRAINT; Schema: auth; Owner: postgres
--

ALTER TABLE ONLY user_claim
    ADD CONSTRAINT user_claim__claim_id_fkey FOREIGN KEY (claim_id) REFERENCES claim(claim_id);


--
-- TOC entry 2204 (class 2606 OID 16660)
-- Name: user_claim user_claim__user_id_fkey; Type: FK CONSTRAINT; Schema: auth; Owner: postgres
--

ALTER TABLE ONLY user_claim
    ADD CONSTRAINT user_claim__user_id_fkey FOREIGN KEY (user_id) REFERENCES "user"(user_id);


--
-- TOC entry 2207 (class 2606 OID 16926)
-- Name: user user_locale_id_fk; Type: FK CONSTRAINT; Schema: auth; Owner: postgres
--

ALTER TABLE ONLY "user"
    ADD CONSTRAINT user_locale_id_fk FOREIGN KEY (locale_id) REFERENCES core.locale(locale_id);


SET search_path = core, pg_catalog;

--
-- TOC entry 2214 (class 2606 OID 16992)
-- Name: object object_type_id_fk; Type: FK CONSTRAINT; Schema: core; Owner: postgres
--

ALTER TABLE ONLY object
    ADD CONSTRAINT object_type_id_fk FOREIGN KEY (object_type_id) REFERENCES object_type(object_type_id);


--
-- TOC entry 2211 (class 2606 OID 16921)
-- Name: object_translation string_locale_id_fk; Type: FK CONSTRAINT; Schema: core; Owner: postgres
--

ALTER TABLE ONLY object_translation
    ADD CONSTRAINT string_locale_id_fk FOREIGN KEY (locale_id) REFERENCES locale(locale_id);


SET search_path = dating, pg_catalog;

--
-- TOC entry 2212 (class 2606 OID 16950)
-- Name: ad__to__perk_object ad_to_perk_object__ad_fk; Type: FK CONSTRAINT; Schema: dating; Owner: postgres
--

ALTER TABLE ONLY ad__to__perk_object
    ADD CONSTRAINT ad_to_perk_object__ad_fk FOREIGN KEY (ad_id) REFERENCES ad(ad_id);


--
-- TOC entry 2213 (class 2606 OID 17211)
-- Name: ad__to__perk_object ad_to_perk_object__perk_object_id_fk; Type: FK CONSTRAINT; Schema: dating; Owner: postgres
--

ALTER TABLE ONLY ad__to__perk_object
    ADD CONSTRAINT ad_to_perk_object__perk_object_id_fk FOREIGN KEY (perk_object_id) REFERENCES core.object(object_id);


SET search_path = geo, pg_catalog;

--
-- TOC entry 2209 (class 2606 OID 16854)
-- Name: place parent_place_id__place_id; Type: FK CONSTRAINT; Schema: geo; Owner: postgres
--

ALTER TABLE ONLY place
    ADD CONSTRAINT parent_place_id__place_id FOREIGN KEY (parent_place_id) REFERENCES place(place_id);


--
-- TOC entry 2210 (class 2606 OID 16864)
-- Name: place place_type_id__place_type; Type: FK CONSTRAINT; Schema: geo; Owner: postgres
--

ALTER TABLE ONLY place
    ADD CONSTRAINT place_type_id__place_type FOREIGN KEY (place_type_id) REFERENCES place_type(place_type_id);


-- Completed on 2020-08-19 11:26:46

--
-- PostgreSQL database dump complete
--

