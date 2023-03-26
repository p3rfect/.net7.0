CREATE TABLE "public.users" (
	"id" serial NOT NULL,
	"login" varchar(255) NOT NULL UNIQUE,
	"password" varchar(255) NOT NULL,
	"role" varchar(255) NOT NULL,
	CONSTRAINT "users_pk" PRIMARY KEY ("id")
) WITH (
  OIDS=FALSE
);



CREATE TABLE "public.speciality" (
	"id" serial NOT NULL,
	"code" varchar(50) NOT NULL UNIQUE,
	"name" varchar(255) NOT NULL,
	"budjet_places" integer NOT NULL,
	"paid_places" integer NOT NULL,
	"targeted_places" integer NOT NULL,
	CONSTRAINT "speciality_pk" PRIMARY KEY ("id")
) WITH (
  OIDS=FALSE
);



CREATE TABLE "public.privileges" (
	"id" integer NOT NULL,
	"type" BOOLEAN
) WITH (
  OIDS=FALSE
);



CREATE TABLE "public.speciality_exams" (
	"speciality_id" integer NOT NULL,
	"exam1_id" integer NOT NULL,
	"exam2_id" integer NOT NULL,
	"exam3_id" integer NOT NULL
) WITH (
  OIDS=FALSE
);



CREATE TABLE "public.exams" (
	"id" serial NOT NULL,
	"name" varchar(255) NOT NULL,
	CONSTRAINT "exams_pk" PRIMARY KEY ("id")
) WITH (
  OIDS=FALSE
);



CREATE TABLE "public.user_specialities" (
	"user_id" integer NOT NULL,
	"priority" integer NOT NULL,
	"speciality_id" integer NOT NULL,
	"user_points" integer NOT NULL
) WITH (
  OIDS=FALSE
);



CREATE TABLE "public.user_exams" (
	"user_id" integer NOT NULL,
	"exam_id" integer NOT NULL,
	"point" integer NOT NULL
) WITH (
  OIDS=FALSE
);



CREATE TABLE "public.user_legal_representatives" (
	"representative_id" serial NOT NULL,
	"user_id" integer NOT NULL,
	"first_name" varchar(255) NOT NULL,
	"second_name" varchar(255) NOT NULL,
	"middle_name" varchar(255) NOT NULL,
	"workplace" varchar(255) NOT NULL,
	"phone_number" integer NOT NULL,
	CONSTRAINT "user_legal_representatives_pk" PRIMARY KEY ("representative_id")
) WITH (
  OIDS=FALSE
);



CREATE TABLE "public.user_info" (
	"user_id" integer NOT NULL,
	"first_name" integer NOT NULL,
	"second_name" integer NOT NULL,
	"middle_name" integer NOT NULL,
	"brithdate" DATE NOT NULL,
	"document_series" varchar(10) NOT NULL,
	"document_type" varchar(255) NOT NULL,
	"document_number" integer NOT NULL,
	"document_issue_date" DATE NOT NULL,
	"document_expire_date" DATE NOT NULL,
	"document_organization" varchar(255) NOT NULL,
	"identification_number" integer NOT NULL,
	"phone_number" integer NOT NULL,
	"educational_institution" varchar(255) NOT NULL,
	"diploma_points" integer NOT NULL,
	"graduation_date" DATE NOT NULL,
	"diploma_series" varchar(10) NOT NULL,
	"diploma_number" integer NOT NULL
) WITH (
  OIDS=FALSE
);





ALTER TABLE "privileges" ADD CONSTRAINT "privileges_fk0" FOREIGN KEY ("id") REFERENCES "users"("id");

ALTER TABLE "speciality_exams" ADD CONSTRAINT "speciality_exams_fk0" FOREIGN KEY ("speciality_id") REFERENCES "speciality"("id");
ALTER TABLE "speciality_exams" ADD CONSTRAINT "speciality_exams_fk1" FOREIGN KEY ("exam1_id") REFERENCES "exams"("id");
ALTER TABLE "speciality_exams" ADD CONSTRAINT "speciality_exams_fk2" FOREIGN KEY ("exam2_id") REFERENCES "exams"("id");
ALTER TABLE "speciality_exams" ADD CONSTRAINT "speciality_exams_fk3" FOREIGN KEY ("exam3_id") REFERENCES "exams"("id");


ALTER TABLE "user_specialities" ADD CONSTRAINT "user_specialities_fk0" FOREIGN KEY ("user_id") REFERENCES "users"("id");
ALTER TABLE "user_specialities" ADD CONSTRAINT "user_specialities_fk1" FOREIGN KEY ("speciality_id") REFERENCES "speciality"("id");

ALTER TABLE "user_exams" ADD CONSTRAINT "user_exams_fk0" FOREIGN KEY ("user_id") REFERENCES "users"("id");
ALTER TABLE "user_exams" ADD CONSTRAINT "user_exams_fk1" FOREIGN KEY ("exam_id") REFERENCES "exams"("id");

ALTER TABLE "user_legal_representatives" ADD CONSTRAINT "user_legal_representatives_fk0" FOREIGN KEY ("user_id") REFERENCES "users"("id");

ALTER TABLE "user_info" ADD CONSTRAINT "user_info_fk0" FOREIGN KEY ("user_id") REFERENCES "users"("id");










